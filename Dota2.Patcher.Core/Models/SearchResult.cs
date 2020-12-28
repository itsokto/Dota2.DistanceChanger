using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Dota2.Patcher.Core.Models
{
	[DebuggerDisplay("[Offset = {Offset}, Value = {Value}]")]
	[Serializable]
	public class SearchResult<T> : INotifyPropertyChanged
	{
		private int _offset;

		public int Offset
		{
			get => _offset;
			set
			{
				_offset = value;
				OnPropertyChanged();
			}
		}

		private T _value;

		public T Value
		{
			get => _value;
			set
			{
				_value = value;
				OnPropertyChanged();
			}
		}

		public override string ToString()
		{
			return $"[Offset = {Offset}, Value = {Value}]";
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}