using System;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel, hvorfra der kan bogføres.
    /// </summary>
    public interface IBogføringViewModel : IViewModel
    {
        /// <summary>
        /// Regnskabet, som bogføringslinjen kan bogføres på.
        /// </summary>
        IRegnskabViewModel Regnskab
        {
            get;
        }

        /// <summary>
        /// Bogføringstidspunkt.
        /// </summary>
        DateTime Dato
        {
            get; 
        }

        /// <summary>
        /// Tekstangivelse af bogføringstidspunkt.
        /// </summary>
        string DatoAsText
        {
            get; 
            set;
        }

        /// <summary>
        /// Angivelse af, om tekstangivelsen for bogføringstidspunktet kan redigeres.
        /// </summary>
        bool DatoAsTextIsReadOnly
        {
            get;
        }

        /// <summary>
        /// Label til bogføringstidspunkt.
        /// </summary>
        string DatoLabel
        {
            get;
        }

        /// <summary>
        /// Bilagsnummer.
        /// </summary>
        string Bilag
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for bilagsnummeret.
        /// </summary>
        int BilagMaxLength
        {
            get;
        }

        /// <summary>
        /// Angivelse af, om bilagsnummeret kan redigeres.
        /// </summary>
        bool BilagIsReadOnly
        {
            get;
        }

        /// <summary>
        /// Label til bilagsnummer.
        /// </summary>
        string BilagLabel
        {
            get; 
        }

        /// <summary>
        /// Kontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string Kontonummer
        {
            get; 
            set; 
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for kontonummeret.
        /// </summary>
        int KontonummerMaxLength
        {
            get;
        }

        /// <summary>
        /// Angivelse af, om kontonummeret kan redigeres.
        /// </summary>
        bool KontonummerIsReadOnly
        {
            get;
        }

        /// <summary>
        /// Label til kontonummeret, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string KontonummerLabel
        {
            get; 
        }

        /// <summary>
        /// Navn på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string Kontonavn
        {
            get;
        }

        /// <summary>
        /// Label til navnet på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string KontonavnLabel
        {
            get;
        }

        /// <summary>
        /// Saldo på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        decimal KontoSaldo
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af saldo på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string KontoSaldoAsText
        {
            get;
        }

        /// <summary>
        /// Label til saldoen på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string KontoSaldoLabel
        {
            get;
        }

        /// <summary>
        /// Disponibel beløb på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        decimal KontoDisponibel
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af disponibel beløb på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string KontoDisponibelAsText
        {
            get;
        }

        /// <summary>
        /// Label til disponibel beløb på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string KontoDisponibelLabel
        {
            get;
        }

        /// <summary>
        /// Tekst.
        /// </summary>
        string Tekst
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for teksten til bogføringslinjen.
        /// </summary>
        int TekstMaxLength
        {
            get;
        }

        /// <summary>
        /// Angivelse af, om teksten til bogføringslinjen kan redigeres.
        /// </summary>
        bool TekstIsReadOnly
        {
            get;
        }

        /// <summary>
        /// Label til teksten på bogføringslinjen.
        /// </summary>
        string TekstLabel
        {
            get; 
        }

        /// <summary>
        /// Kontonummer på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string Budgetkontonummer
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af den maksimale tekstlængde for kontonummeret på budgetkontoen.
        /// </summary>
        int BudgetkontonummerMaxLength
        {
            get;
        }

        /// <summary>
        /// Angivelse af, om kontonummeret på budgetkontoen kan redigeres.
        /// </summary>
        bool BudgetkontonummerIsReadOnly
        {
            get;
        }

        /// <summary>
        /// Label til kontonummeret på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string BudgetkontonummerLabel
        {
            get;
        }

        /// <summary>
        /// Navn på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string Budgetkontonavn
        {
            get;
        }

        /// <summary>
        /// Label til navnet på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string BudgetkontonavnLabel
        {
            get;
        }

        /// <summary>
        /// Bogført beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        decimal BudgetkontoBogført
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af bogført beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string BudgetkontoBogførtAsText
        {
            get;
        }

        /// <summary>
        /// Label til bogført beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string BudgetkontoBogførtLabel
        {
            get;
        }

        /// <summary>
        /// Disponibel beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        decimal BudgetkontoDisponibel
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af disponibel beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string BudgetkontoDisponibelAsText
        {
            get;
        }

        /// <summary>
        /// Label til disponibel beløb på budgetkontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string BudgetkontoDisponibelLabel
        {
            get;
        }
    }
}
