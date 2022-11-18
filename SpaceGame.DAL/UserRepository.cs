using Amazon.DynamoDBv2.DataModel;
using SpaceGame.DAL.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Amazon.DynamoDBv2.DocumentModel;
using AmazonExpression = Amazon.DynamoDBv2.DocumentModel.Expression;

namespace SpaceGame.DAL
{
    public interface IUserRepository
    {
        Task<User> GetById(string login);
        Task PutItem(User user);
        Task<IEnumerable<User>> Get(DateTime periodAccessing);
        Task Update(User user);
        Task<User> GetByNickName(string nickName);
        Task Delete(User user);
    }

    internal class UserRepository : IUserRepository
    {
        private readonly IDynamoDBContext _context;
        private readonly Table _userTable;

        public UserRepository(IDynamoDBContext context)
        {
            _context = context;
            _userTable = _context.GetTargetTable<User>();
        }

        public async Task<User> GetById(string email)
        {
            var user = await _userTable.GetItemAsync(new Primitive(email));
            return _context.FromDocument<User>(user);
        }

        // Home work NickName
        public async Task<User> GetByNickName(string nickName)
        {
            return new User();// delete this row and add logic to scan by nickname
        }

        public async Task<IEnumerable<User>> Get(DateTime periodAccessing)
        {
            List<ScanCondition> conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition(nameof(User.AccessTime), ScanOperator.GreaterThanOrEqual, periodAccessing));
            var result = _context.ScanAsync<User>(conditions);
            IEnumerable<User> users = await result.GetRemainingAsync();

            return users;
        }

        public async Task Update(User user)
        {
            var doc = _context.ToDocument<User>(user);
            await _userTable.UpdateItemAsync(doc);
        }

        public async Task PutItem(User user)
        {
            var userDoc = _context.ToDocument(user);
            var requestConfig = new PutItemOperationConfig
            {
                ConditionalExpression = new AmazonExpression
                {
                    ExpressionStatement = "attribute_not_exists(email)"
                },
                ReturnValues = ReturnValues.UpdatedNewAttributes
            };

            await _userTable.PutItemAsync(userDoc,requestConfig);
        }

        // Home work
        public async Task Delete(User user)
        {
        }
    }
}