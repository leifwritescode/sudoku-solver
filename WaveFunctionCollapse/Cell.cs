namespace WaveFunctionCollapse;

internal class Cell : ICloneable
{
    private List<int> _possibleValues = [];

    public bool IsCollapsed => Value.HasValue;

    public int? Value => _possibleValues.Count == 1 ? _possibleValues[0] : null;

    public int Entropy => _possibleValues.Count;

    public Cell()
    {
        _possibleValues.AddRange([1, 2, 3, 4, 5, 6, 7, 8, 9]);
    }

    public Cell(int value)
    {
        if (value < 0 || value > 9)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        if (value == 0)
        {
            _possibleValues.AddRange([1, 2, 3, 4, 5, 6, 7, 8, 9]);
        }
        else
        {
            _possibleValues.Add(value);
        }
    }

    public void Eliminate(int value)
    {
        if (Value.HasValue)
        {
            return;
        }

        _possibleValues.Remove(value);
    }

    public void Collapse()
    {
        if (Value.HasValue)
        {
            return;
        }

        var extent = _possibleValues.Count;
        var index = Random.Shared.Next(extent);
        var value = _possibleValues[index];
        _possibleValues.Clear();
        _possibleValues.Add(value);
    }

    public override string ToString()
    {
        if (Value.HasValue)
        {
            return $"{Value}";
        }

        return string.Join(',', _possibleValues);
    }

    public object Clone()
    {
        return new Cell
        {
            _possibleValues = new List<int>(_possibleValues)
        };
    }
}
