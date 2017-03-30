using System;
using System.Activities.Debugger;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ksj.Mealplan.Domain;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Ksj.Mealplan.Infrastructure
{
    public abstract class BaseRepository<T>
    {
        private readonly IReliableStateManager _stateManager;
        private readonly string _collectionName;

        protected BaseRepository(IReliableStateManager stateManager, string collectionName)
        {
            _stateManager = stateManager;
            _collectionName = collectionName;
        }

        public async Task SaveAsync(T entity, string id)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var collection =  await _stateManager.GetOrAddAsync<IReliableDictionary<string, T>>(_collectionName);
                var collectionEntity = await collection.TryGetValueAsync(tx, id);
                if (collectionEntity.Value != null)
                    await collection.TryUpdateAsync(tx, id, entity, collectionEntity.Value);
                else
                    await collection.AddAsync(tx, id.ToLower(), entity);
                await tx.CommitAsync();
            }
        }

        public async Task<T> GetEntityAsync(string id)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var collection = await _stateManager.GetOrAddAsync<IReliableDictionary<string, T>>(_collectionName);
                var response =  await collection.TryGetValueAsync(tx, id);
                return response.Value;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var coll = new List<T>();
            using (var tx = _stateManager.CreateTransaction())
            {
                var collection = await _stateManager.GetOrAddAsync<IReliableDictionary<string, T>>(_collectionName);
                var response = await collection.CreateEnumerableAsync(tx);
                var enumerator = response.GetAsyncEnumerator();
                while (await enumerator.MoveNextAsync(new CancellationToken(false)))
                {
                    coll.Add(enumerator.Current.Value);
                }
                return coll;
            }
        }
    }
}
