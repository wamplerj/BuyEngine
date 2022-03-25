using System.Collections;

// ReSharper disable PossibleMultipleEnumeration

namespace BuyEngine.Common
{
    [Serializable]
    public class PagedList<T> : IReadOnlyList<T>, IPagedList<T>
    {
        private readonly List<T> _items;

        public int TotalCount { get; private set; }
        public int Page { get; private set; }
        public int TotalPages
        {
            get
            {
                var result = decimal.Divide(TotalCount, PageSize);
                var pagesAsDecimal = Math.Floor(result) + 1;

                return (int)pagesAsDecimal;
            }
        }

        public int PageSize { get; private set; }

        public PagedList(IEnumerable<T> items, int pageSize, int page, int totalCount)
        {
            Guard.Null(items, nameof(items));
            Guard.Negative(pageSize, nameof(pageSize));
            Guard.Negative(page, nameof(page));
            Guard.Negative(totalCount, nameof(totalCount));

            _items = new List<T>(items);
            PageSize = pageSize;
            Page = page;
            TotalCount = totalCount;
        }

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(T item) => _items.Contains(item);

        public int Count => _items.Count;

        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }

        public T this[int index] => _items[index];
    }

    public static class PagesListExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> list, int pageSize, int page, int totalCount)
        {
            return new(list, pageSize, page, totalCount);
        }

        public static int SkipCount(this int page, int pageSize) => pageSize * (page - 1);
    }

    public interface IPagedList<T> : IEnumerable<T>
    {
        int TotalCount { get; }
        int Page { get; }
        int TotalPages { get; }
        int PageSize { get; }
        int Count { get; }
        bool Contains(T item);
        int IndexOf(T item);
        T this[int index] { get; }
    }
}
