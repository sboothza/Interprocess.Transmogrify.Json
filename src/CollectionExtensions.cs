using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
namespace Interprocess.Transmogrify.Json
{
	public static class CollectionExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<DictionaryEntry> ToList(this IDictionary dictionary)
		{
			try
			{
				var list = new List<DictionaryEntry>();
				var enu = dictionary.GetEnumerator();
				while (enu.MoveNext())
				{
					var entry = enu.Current;
					if (entry != null)
					{
						var entryObj = (DictionaryEntry)entry;
						list.Add(entryObj);
					}
				}

				return list;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public static void ProcessList<T>(this IEnumerable<T> enumerable, Action<T> action, Action<T> lastAction)
		{
			try
			{
				using (var enu = enumerable.GetEnumerator())
				{
					bool canMove = enu.MoveNext();
					T? last = default;
					while (canMove)
					{
						last = enu.Current;
						canMove = enu.MoveNext();
						if (canMove)
							action(last);
					}

					if (last is not null)
						lastAction(last);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				throw;
			}
		}
	}
}