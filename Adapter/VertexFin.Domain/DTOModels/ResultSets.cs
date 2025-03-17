using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace VertexFin.Domain.DTOModels
{
    public class ResultSets
    {
        private List<ResultSet>? _resultSets;

        public List<ResultSet>? GetResultSets
        {
            get { return _resultSets; }
        }

        public ResultSets(List<ResultSet>? resultSets)
        {
            _resultSets = resultSets;
        }

        public ResultSet GetResultSetByIndex(int index)
        {
            if (_resultSets == null)
            {
                throw new ArgumentNullException(nameof(_resultSets));
            }

            if (index < 0 || index >= _resultSets.Count)
            { 
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _resultSets[index];
        }
     }

    public class ResultSet
    {
        private List<ResultSetRow>? _resultSetRows;

        public List<ResultSetRow>? GetResultSetRows
        {
            get { return _resultSetRows; }
        }
        public ResultSet(List<ResultSetRow>? resultSetRows)
        {
            _resultSetRows = resultSetRows;
        }

        public ResultSetRow GetResultSetRowByIndex(int index)
        {
            if (_resultSetRows == null)
            {
                throw new ArgumentNullException(nameof(_resultSetRows));
            }

            if (index < 0 || index >= _resultSetRows.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _resultSetRows[index];
        }

        public List<T?> DeserializeResultSetObject<T>()
        {
            List<T?> values = new List<T?>();
            foreach (ResultSetRow resultSetRow in _resultSetRows ?? [])
            {
                values.Add(resultSetRow.DeserializeResultSetRowObject<T>());
            }
            return values;
        }
    }

    public class ResultSetRow
    {
        private Dictionary<string, object?>? _columns;

        public Dictionary<string, object?>? GetColumns
        {
            get { return _columns; }
        }

        public ResultSetRow(Dictionary<string, object?>? columns)
        {
            _columns = columns;
        }

        public T? GetColumnValueByName<T>(string name) where T : struct
        {
            if (_columns == null)
            {
                throw new ArgumentNullException(nameof(_columns));
            }

            if (!_columns.ContainsKey(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name));
            }

            return (T?)_columns[name];
        }

        public T? DeserializeResultSetRowObject<T>()
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(_columns));
        }
    }
}
