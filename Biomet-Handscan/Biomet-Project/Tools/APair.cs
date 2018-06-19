namespace LSTools
{
    public struct APair<T0, T1>
    {
        public T0 First;
        public T1 Second;

        public APair(T0 first, T1 second)
        {
            First = first;
            Second = second;
        }
    }
}