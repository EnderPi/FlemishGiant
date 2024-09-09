using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnderPi.SystemE
{
    public class BatchDeduplicator<T>
    {
        private Func<IEnumerable<T>> _getNext;
        private Func<T, long> _getHashCode;
        private HashSet<long> _alreadyRetrieved;

        public BatchDeduplicator(Func<IEnumerable<T>> getTasks, Func<T, long> getHashCode ) 
        { 
            _getNext = getTasks;
            _getHashCode = getHashCode;
            _alreadyRetrieved = new HashSet<long>();
        }

        public List<T> GetNext()
        {
            var nextBatch = _getNext();
            List<T> result = new List<T>();
            foreach ( var item in nextBatch )
            {
                if (_alreadyRetrieved.Add(_getHashCode( item )) )
                {
                    result.Add( item );
                }
            }
            return result;
        }
    }
}
