using LastTrade.Application.RepoContract;
using LastTrade.Domain.Entities;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using LastTrade.Application.DTOs;

namespace LastTrade.Data.Repositories
{
    public class TradeRepo : ITradeRepo

    {
        private readonly IConfiguration _configuration;

        public TradeRepo(IConfiguration configuration)
        {
            this._configuration = configuration;
        }


        private bool IsExistDatabase(string DatbaseName)
        {

            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(_configuration.GetConnectionString("SqlServerConnection"));
            sqlConnectionStringBuilder.InitialCatalog = "master";
            using (var connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            using (var command = new SqlCommand("SELECT db_id(@databaseName)", connection))
            {
                command.Parameters.Add(new SqlParameter("databaseName", DatbaseName));

                connection.Open();

                return (command.ExecuteScalar() != DBNull.Value);
            }
        }



        public void CreateTable()
        {

            string CreateDB = @"
            CREATE DATABASE [TradeDB]
             CONTAINMENT = NONE
             ON  PRIMARY 
            ( NAME = N'TradeDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\TradeDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
             LOG ON 
            ( NAME = N'TradeDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\TradeDB.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
                        
            ";


            string createtable = @"
            
            CREATE TABLE [dbo].[Instrument](
            	[Id] [int] NULL,
            	[Shortname] [nvarchar](20) NULL
            ) ON [PRIMARY]
            
            
            
            CREATE TABLE [dbo].[Trade](
            	[Id] [int] NULL,
            	[InstrumentId] [int] NULL,
            	[DateTimeEn] [datetime] NULL,
            	[Open] [decimal](18, 4) NULL,
            	[High] [decimal](18, 4) NULL,
            	[Low] [decimal](18, 4) NULL,
            	[Close] [decimal](18, 4) NULL
            ) ON [PRIMARY]
            
            
            INSERT [dbo].[Instrument] ([Id], [Shortname]) VALUES (1, N'AAPL')
            
            INSERT [dbo].[Instrument] ([Id], [Shortname]) VALUES (2, N'OGL')
            
            INSERT [dbo].[Trade] ([Id], [InstrumentId], [DateTimeEn], [Open], [High], [Low], [Close]) VALUES (1, 1, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(1001.0000 AS Decimal(18, 4)), CAST(2001.0000 AS Decimal(18, 4)), CAST(301.0000 AS Decimal(18, 4)), CAST(401.0000 AS Decimal(18, 4)))
            
            INSERT [dbo].[Trade] ([Id], [InstrumentId], [DateTimeEn], [Open], [High], [Low], [Close]) VALUES (2, 1, CAST(N'2020-01-02T00:00:00.000' AS DateTime), CAST(1002.0000 AS Decimal(18, 4)), CAST(2002.0000 AS Decimal(18, 4)), CAST(302.0000 AS Decimal(18, 4)), CAST(402.0000 AS Decimal(18, 4)))
            
            INSERT [dbo].[Trade] ([Id], [InstrumentId], [DateTimeEn], [Open], [High], [Low], [Close]) VALUES (3, 1, CAST(N'2020-01-03T00:00:00.000' AS DateTime), CAST(1003.0000 AS Decimal(18, 4)), CAST(2003.0000 AS Decimal(18, 4)), CAST(303.0000 AS Decimal(18, 4)), CAST(403.0000 AS Decimal(18, 4)))
            
            INSERT [dbo].[Trade] ([Id], [InstrumentId], [DateTimeEn], [Open], [High], [Low], [Close]) VALUES (4, 2, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(1004.0000 AS Decimal(18, 4)), CAST(2004.0000 AS Decimal(18, 4)), CAST(304.0000 AS Decimal(18, 4)), CAST(404.0000 AS Decimal(18, 4)))
            
            INSERT [dbo].[Trade] ([Id], [InstrumentId], [DateTimeEn], [Open], [High], [Low], [Close]) VALUES (5, 2, CAST(N'2020-01-03T00:00:00.000' AS DateTime), CAST(1005.0000 AS Decimal(18, 4)), CAST(2005.0000 AS Decimal(18, 4)), CAST(305.0000 AS Decimal(18, 4)), CAST(405.0000 AS Decimal(18, 4)))
            
            INSERT [dbo].[Trade] ([Id], [InstrumentId], [DateTimeEn], [Open], [High], [Low], [Close]) VALUES (6, 1, CAST(N'2021-01-01T00:00:00.000' AS DateTime), CAST(1007.0000 AS Decimal(18, 4)), CAST(2007.0000 AS Decimal(18, 4)), CAST(307.0000 AS Decimal(18, 4)), CAST(407.0000 AS Decimal(18, 4)))
            
            
            ";

            SqlConnectionStringBuilder OldsqlConnectionStringBuilder = new SqlConnectionStringBuilder(_configuration.GetConnectionString("SqlServerConnection"));
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(_configuration.GetConnectionString("SqlServerConnection"));

            if (!IsExistDatabase(sqlConnectionStringBuilder.InitialCatalog))
            {

               
                sqlConnectionStringBuilder.InitialCatalog = "master";
                using (SqlConnection dbConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
                {

                    using (var command = new SqlCommand(CreateDB, dbConnection))
                    {
                        dbConnection.Open();

                        command.ExecuteScalar();
                    }

                }

              
                using (SqlConnection dbConnection = new SqlConnection(OldsqlConnectionStringBuilder.ConnectionString))
                {

                    using (var command = new SqlCommand(createtable, dbConnection))
                    {
                        dbConnection.Open();

                        command.ExecuteScalar();


                    }

                }


            }




        }
    


       public async Task<IEnumerable<Trade>> GetLastTradeAsync(DateTime? startDate)
        {
          
            IEnumerable<Trade> lastTradsDTOs;
            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("SqlServerConnection")))
            {
                var dynamicParameters = new DynamicParameters();
                if (startDate != null)
                {
                    dynamicParameters.Add("TradeDate", startDate);
                }
                else
                {
                    dynamicParameters.Add("TradeDate", new DateTime(1900, 01, 01));
                }

                dbConnection.Open();
                lastTradsDTOs = await dbConnection.QueryAsync<Trade, Instrument, Trade>(@"
                SELECT Trade.*, Instrument.*
                FROM Instrument INNER JOIN
                Trade ON Instrument.Id = Trade.InstrumentId
                
                ,(SELECT InstrumentId, MAX(DateTimeEn) AS DateTimeEn
                FROM  Trade
                WHERE DateTimeEn>=@TradeDate
                GROUP BY InstrumentId ) LastTrade

                WHERE Trade.InstrumentId=LastTrade.InstrumentId 
                AND Trade.DateTimeEn=LastTrade.DateTimeEn
            
            ", (Trade, Instrument) =>
                {
                    Trade.Instrument = Instrument;
                    return Trade;
                }, dynamicParameters);
                dbConnection.Close();
            }

            return lastTradsDTOs;
        }

      
    }
}
