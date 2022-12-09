using Monopoly;

public interface IText<T>
{
	Coord Coord { get; }
	string Value { get; }
	T TextStyle { get; }

	void Print();
	void Update(string text);

	void Clear();
}
