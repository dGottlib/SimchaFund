using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Class50_SimchaFund.Data
{
    public class SimchaFundDB
    {
        private string _connectionString;

        public SimchaFundDB(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int AddContributor(Contributor contributor)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Contributors (FirstName, LastName, Cell, AlwaysInclude)
                                VALUES (@firstName, @lastName, @cell, @alwaysInclude)
                                SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@firstName", contributor.FirstName);
            cmd.Parameters.AddWithValue("@lastName", contributor.LastName);
            cmd.Parameters.AddWithValue("@cell", contributor.Cell);
            cmd.Parameters.AddWithValue("@alwaysInclude", contributor.AlwaysInclude);
            connection.Open();
            return (int)(decimal)cmd.ExecuteScalar();
        }
        public void AddDeposit(Deposit deposit)
        {
            AddDeposit(deposit, deposit.ContributorId);
        }
        public void AddDeposit(Deposit deposit, int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Deposits(ContributorId, Amount, Date)
                                VALUES (@contributorId, @amount, @date)";
            cmd.Parameters.AddWithValue("@contributorId", contributorId);
            cmd.Parameters.AddWithValue("@amount", deposit.Amount);
            cmd.Parameters.AddWithValue("@date", deposit.Date);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public List<Contributor> GetContributors()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Contributors";
            connection.Open();
            var reader = cmd.ExecuteReader();
            var contributors = new List<Contributor>();

            while (reader.Read())
            {
                contributors.Add(new Contributor
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    Cell = (string)reader["Cell"],
                    AlwaysInclude = (bool)reader["AlwaysInclude"],
                    Balance = GetBalanceForContributor((int)reader["Id"])
                });
            }
            return contributors;
        }
        public void UpdateContributor(Contributor contributor)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Contributors SET FirstName = @firstName, LastName = @lastName,
                                Cell = @cell, AlwaysInclude = @alwaysInclude
                                WHERE Id = @id";
            cmd.Parameters.AddWithValue("@firstName", contributor.FirstName);
            cmd.Parameters.AddWithValue("@lastName", contributor.LastName);
            cmd.Parameters.AddWithValue("@cell", contributor.Cell);
            cmd.Parameters.AddWithValue("@alwaysInclude", contributor.AlwaysInclude);
            cmd.Parameters.AddWithValue("@id", contributor.Id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public decimal GetBalanceForContributor(int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT ISNULL(SUM(d.Amount), 0) - ISNULL(SUM(co.Amount), 0) AS 'Balance', c.Id
                                FROM Contributors c
                                LEFT JOIN Contributions co
                                ON c.Id = co.ContributorId
                                LEFT JOIN Deposits d
                                ON c.Id = d.ContributorId 
                                WHERE c.Id = @id
                                GROUP BY c.Id";
            cmd.Parameters.AddWithValue("@id", contributorId);
            connection.Open();
            var reader = cmd.ExecuteReader();
            reader.Read();

            return (decimal)reader["Balance"];
        }
        public decimal TotalFunds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT ISNULL(SUM(d.Amount) - SUM(co.Amount), SUM(d.amount)) AS 'Balance'
                                FROM contributors c
                                LEFT JOIN contributions co
                                ON c.ID = co.ContributorID
                                JOIN Deposits d
                                ON d.ContributorID = c.ID";
            connection.Open();

            return (decimal)cmd.ExecuteScalar();
        }
        public List<Transaction> GetHistory(int contributorId)
        {
            var transactions = (GetDepositsForContributor(contributorId));
            transactions.AddRange(GetContributionsForContributor(contributorId));
            return transactions.OrderByDescending(t => t.Date).ToList();

        }
        public List<Transaction> GetDepositsForContributor(int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Deposits WHERE ContributorId = @id";
            cmd.Parameters.AddWithValue("@id", contributorId);
            connection.Open();
            var reader = cmd.ExecuteReader();
            var transactions = new List<Transaction>();

            while (reader.Read())
            {
                transactions.Add(new Transaction
                {
                    Action = ("Deposit"),
                    Amount = (decimal)reader["Amount"],
                    Date = (DateTime)reader["Date"]
                });
            }
            return transactions;
        }
        public List<Transaction> GetContributionsForContributor(int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT c.Amount, s.Name, s.Date from Contributions c
                                JOIN Simchas s
                                ON s.Id = c.SimchaId
                                WHERE c.ContributorId =  @id";
            cmd.Parameters.AddWithValue("@id", contributorId);
            connection.Open();
            var reader = cmd.ExecuteReader();
            var transactions = new List<Transaction>();

            while (reader.Read())
            {
                transactions.Add(new Transaction
                {
                    Action = ($"Contribution for the {(string)reader["Name"]}"),
                    Date = (DateTime)reader["Date"],
                    Amount = (decimal)reader["Amount"]
                });
            }
            return transactions;
        }
        public int AddSimcha(Simcha simcha)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Simchas (Name, Date)
                                VALUES (@name, @date)
                                SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@name", simcha.Name);
            cmd.Parameters.AddWithValue("@date", simcha.Date);
            connection.Open();
            return (int)(decimal)cmd.ExecuteScalar();
        }
        public List<Simcha> GetSimchas()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT s.Id, s.Name, s.Date, COUNT(c.amount) AS 'TotalContributorsForSimcha', ISNULL(SUM(c.amount), 0) AS 'TotalContributed'
                                FROM Simchas s
                                LEFT JOIN Contributions c
                                ON s.Id = c.SimchaId
                                GROUP BY s.Id, s.Name, s.Date";
            connection.Open();
            var reader = cmd.ExecuteReader();
            var simchas = new List<Simcha>();

            while (reader.Read())
            {
                simchas.Add(new Simcha
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    TotalContributorsForSimcha = (int)reader["TotalContributorsForSimcha"],
                    TotalContributed = (decimal)reader["TotalContributed"]
                });
            }
            return simchas;
        }
        public List<Contributor> GetContributions(int simchaId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT c.*, co.Amount FROM Contributors c
                                LEFT JOIN Contributions co
                                ON c.Id = co.ContributorId
                                LEFT JOIN Simchas s
                                ON s.Id = co.SimchaId
                                WHERE s.Id = @id";
            cmd.Parameters.AddWithValue("@id", simchaId);
            connection.Open();
            var reader = cmd.ExecuteReader();


            var contributorsForSimcha = new List<Contributor>();
            while (reader.Read())
            {
                contributorsForSimcha.Add(new Contributor
                {
                    Id = (int)reader["Id"],
                    Amount = (decimal)reader["Amount"]
                });
            }

            var contributors = new List<Contributor>();
            var getContributors = GetContributors();

            foreach (Contributor c in getContributors)
            {
                var contributor = new Contributor
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    AlwaysInclude = c.AlwaysInclude,
                    Balance = GetBalanceForContributor(c.Id)
                };

                if (contributorsForSimcha.Select(s => s.Id).Any(s => s == c.Id))
                {
                    contributor.Include = true;
                    contributor.Amount = contributorsForSimcha.FirstOrDefault(s => s.Id == c.Id).Amount;
                }
                else
                {
                    contributor.Include = false;
                    contributor.Amount = 0;
                }

                contributors.Add(contributor);
            }
            return contributors;
        }
        public void UpdateContributions(List<Contributor> contributors, int simchaId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            DeleteForSimcha(simchaId);
            foreach (Contributor contributor in contributors)
            {
                connection.Close();
                cmd.Parameters.Clear();
                cmd.CommandText = @"INSERT INTO Contributions(ContributorId, SimchaId, Amount)
                                    VALUES(@contributorId, @simchaId, @amount)";
                cmd.Parameters.AddWithValue("@contributorId", contributor.Id);
                cmd.Parameters.AddWithValue("@simchaId", simchaId);
                cmd.Parameters.AddWithValue("@amount", contributor.Amount);


                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteForSimcha(int simchaid)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"delete from contributions where SimchaId= @Id";
            cmd.Parameters.AddWithValue("@Id", simchaid);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }
}
