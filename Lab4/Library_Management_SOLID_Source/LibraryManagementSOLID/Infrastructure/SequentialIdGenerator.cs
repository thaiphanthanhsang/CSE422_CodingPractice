using LibraryManagementSOLID.Interfaces;

namespace LibraryManagementSOLID.Infrastructure;

public sealed class SequentialIdGenerator : IIdGenerator
{
    private int _nextId;

    public SequentialIdGenerator(int firstId = 1)
    {
        if (firstId <= 0) throw new ArgumentOutOfRangeException(nameof(firstId));
        _nextId = firstId;
    }

    public int NextId() => _nextId++;
}
