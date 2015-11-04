namespace FileWordsDataflow.Common
{
    public interface IModuleRegistrar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "It is OK")]
        void RegisterType<TFrom, TTo>() where TTo : TFrom;
    }
}