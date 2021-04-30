using System;
using System.Collections;
using System.Collections.Generic;
// ReSharper disable PossibleMultipleEnumeration

namespace BuyEngine.Common
{
    [Serializable]
    public class PagedList<T> : IList<T>
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


        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new System.NotImplementedException();
        }

        public int Count => _items.Count;
        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public T this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }
    }

    public static class PagesListExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> list, int pageSize, int page, int totalCount)
        {
            return new(list, pageSize, page, totalCount);
        }
    }
}
