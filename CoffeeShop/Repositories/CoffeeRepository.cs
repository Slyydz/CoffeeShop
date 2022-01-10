using CoffeeShop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly string _connectionString;
        public CoffeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }

        public List<Coffee> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Coffee.Id 'CoffeeId', Coffee.Title 'CoffeeTitle', BeanVariety.Id 'BeanVarietyId', BeanVariety.Region, BeanVariety.Name 'BeanVarietyName', BeanVariety.Notes FROM Coffee JOIN BeanVariety ON BeanVariety.Id = Coffee.BeanVarietyId";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var varieties = new List<Coffee>();
                        while (reader.Read())
                        {
                            var variety = new Coffee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CoffeeId")),
                                Title = reader.GetString(reader.GetOrdinal("CoffeeTitle")),
                                BeanVariety = new BeanVariety
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                    Region = reader.GetString(reader.GetOrdinal("Region")),
                                    Name = reader.GetString(reader.GetOrdinal("BeanVarietyName")),
                                }
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                            {
                                variety.BeanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                            }

                            varieties.Add(variety);
                        };

                        return varieties;
                    }
                }
            }
        }

        public Coffee Get(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Coffee.Id 'CoffeeId', * FROM Coffee JOIN BeanVariety ON BeanVariety.Id = Coffee.BeanVarietyId WHERE Coffee.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Coffee coffee = null;
                        if (reader.Read())
                        {
                            coffee = new Coffee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CoffeeId")),
                                Title = reader.GetString(reader.GetOrdinal("CoffeeTitle")),
                                BeanVariety = new BeanVariety
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                    Region = reader.GetString(reader.GetOrdinal("Region")),
                                    Name = reader.GetString(reader.GetOrdinal("BeanVarietyName")),
                                }
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                            {
                                coffee.BeanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                            }
                        }
                        return coffee;
                    }
                }
            }
        }

        public void Add(Coffee coffee)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Coffee (Title, BeanVarietyId)
                        OUTPUT INSERTED.ID
                        VALUES (@title, @beanVarietyId)";
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);

                    coffee.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Coffee coffee)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Coffee 
                           SET Title = @title, 
                               BeanVarietyId = @beanVarietyId, 
                         WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", coffee.Id);
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Coffee WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
