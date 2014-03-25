namespace OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en model, der indeholder en advarsel ved en bogføring.
    /// </summary>
    public interface IBogføringsadvarselModel : IModel
    {
        /// <summary>
        /// Tekstangivelse af advarslen, som er opstået ved bogføring.
        /// </summary>
        string Advarsel
        {
            get;
        }

        /// <summary>
        /// Kontonummer på kontoen, hvorpå advarslen er opstået ved bogføring.
        /// </summary>
        string Kontonummer
        {
            get;
        }

        /// <summary>
        /// Kontonavnet på kontoen, hvorpå advarslen er opstået ved bogføring.
        /// </summary>
        string Kontonavn
        {
            get;
        }

        /// <summary>
        /// Beløbet, der har medført advarslen.
        /// </summary>
        decimal Beløb
        {
            get;
        }
    }
}
