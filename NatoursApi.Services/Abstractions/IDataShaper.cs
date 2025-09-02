using System.Dynamic;

namespace NatoursApi.Services.Abstractions;

public interface IDataShaper<T>
{
    IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldsString);

    ExpandoObject ShapeData(T entity, string fieldsString);
}