using Dapper;
using FunctionApp.IsolatedDemo.Api.Entities;
using FunctionApp.IsolatedDemo.Api.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp.IsolatedDemo.Api.Persistence.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly DatabaseInitializer _databaseInitializer;

        public NoteRepository(IDbConnectionFactory connectionFactory, DatabaseInitializer databaseInitializer)
        {
            _connectionFactory = connectionFactory;
            _databaseInitializer = databaseInitializer;
        }

        public async Task<Note> CreateAsync(Note note, CancellationToken ct = default)
        {
            await _databaseInitializer.InitializeAsync();

            using var connection = await _connectionFactory.CreateConnectionAsync(ct);

            var result = await connection.ExecuteAsync(
                @"
                    INSERT INTO Notes (Id, Title, Body, CreatedAt, LastUpdatedOn) 
                    VALUES (@Id, @Title, @Body, @CreatedAt, @LastUpdatedOn)
                ",
                note
            );

            if (result == 1)
            {
                return note;
            }

            throw new ApplicationException("Could not save entity in db.");
        }
    }
}
