namespace WaveFunctionCollapse
{
    public interface IInputReader<T>
    {
        IValue<T>[][] ReadInputToGrid();
    }
}