namespace OSDevGrp.OSIntranet.Gui.Resources
{
    /// <summary>
    /// Exception messages.
    /// </summary>
    public enum ExceptionMessage
    {
        [DefaultMessage("Error in repository method '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error in repository method '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Fejl i repositorymetode '{0}': {1}")]
        RepositoryError,

        [DefaultMessage("Error while executing the command '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error while executing the command '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Fejl ved udførelse af kommandoen '{0}': {1}")]
        CommandError,

        [DefaultMessage("Illegal value for '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.English, "Illegal value for '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Illegal værdi for '{0}': {1}")]
        IllegalArgumentValue,

        [DefaultMessage("Unable to load the resource named '{0}'.")]
        [GlobalizedMessage(CultureInfoProvider.English, "Unable to load the resource named '{0}'.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke loade ressourcen med navnet '{0}'.")]
        UnableToLoadResource,

        [DefaultMessage("The configuration setting named '{0}' is not defined.")]
        [GlobalizedMessage(CultureInfoProvider.English, "The configuration setting named '{0}' is not defined.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Konfigurationsværdien med navnet '{0}' er ikke defineret.")]
        MissingConfigurationSetting,

        [DefaultMessage("The value for the configuration setting named '{0}' is invalid.")]
        [GlobalizedMessage(CultureInfoProvider.English, "The value for the configuration setting named '{0}' is invalid.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Værdien for konfigurationen med navnet '{0}' er illegal.")]
        InvalidConfigurationSettingValue,

        [DefaultMessage("Error while getting value of the property named '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error while getting value of the property named '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Fejl ved returnering af værdi for propertyen med navnet '{0}': {1}")]
        ErrorWhileGettingPropertyValue,

        [DefaultMessage("Error while setting value of the property named '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error while setting value of the property named '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Fejl ved opdatering af værdi for propertyen med navnet '{0}': {1}")]
        ErrorWhileSettingPropertyValue,

        [DefaultMessage("Error in method '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error in method '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Fejl i metode '{0}': {1}")]
        MethodError,

        [DefaultMessage("The account group with the unique number '{0}' could not be found.")]
        [GlobalizedMessage(CultureInfoProvider.English, "The account group with the unique number '{0}' could not be found.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke finde en kontogruppe med det unikke nummer: {0}")]
        AccountGroupNotFound,

        [DefaultMessage("The budget account group with the unique number '{0}' could not be found.")]
        [GlobalizedMessage(CultureInfoProvider.English, "The budget account group with the unique number '{0}' could not be found.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke finde en kontogruppe til budgetkonti med den unikke nummer: {0}")]
        BudgetAccountGroupNotFound,

        [DefaultMessage("Error while setting date for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error while setting date for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke sætte datoen på bogføringslinjen.")]
        ErrorWhileSettingPostingDate,

        [DefaultMessage("Error while setting reference for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error while setting reference for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke sætte bilagsnummer på bogføringslinjen.")]
        ErrorWhileSettingReference,

        [DefaultMessage("Error while setting account number for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error while setting account number for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke sætte kontonummeret på bogføringslinjen.")]
        ErrorWhileSettingAccountNumber,

        [DefaultMessage("Error while setting text for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error while setting text for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke sætte teksten på bogføringslinjen.")]
        ErrorWhileSettingPostingText,

        [DefaultMessage("Error while setting budget account number for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error while setting budget account number for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke sætte kontonummeret for budgetkontoen på bogføringslinjen.")]
        ErrorWhileSettingBudgetAccountNumber,

        [DefaultMessage("Error while setting debit for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error while setting debit for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke sætte debitbeløbet på bogføringslinjen.")]
        ErrorWhileSettingDebit,

        [DefaultMessage("Error while setting credit for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error while setting credit for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke sætte kreditbeløbet på bogføringslinjen.")]
        ErrorWhileSettingCredit,

        [DefaultMessage("Error while setting address account for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error while setting address account for the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke sætte adressekontoen på bogføringslinjen.")]
        ErrorWhileSettingAddressAccount,

        [DefaultMessage("Error when posting the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.English, "Error when posting the accounting line.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Fejl ved bogføring af bogføringslinjen.")]
        ErrorPostingAccountingLine,

        [DefaultMessage("An error occurred while updating through the repository supporting financial management.")]
        [GlobalizedMessage(CultureInfoProvider.English, "An error occurred while updating through the repository supporting financial management.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Der opstod en fejl ved opdatering gennem repositoryet, der supporterer finansstyring.")]
        ErrorUpdateFinansstyringRepository,

        [DefaultMessage("The repository supporting financial management was offline while calling '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.English, "The repository supporting financial management was offline while calling '{0}': {1}")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Repositoryet, der supporterer finansstyring, var offline ved kaldet af '{0}': {1}")]
        OfflineFinansstyringRepository,

        [DefaultMessage("The event handler for '{0}' is not defined.")]
        [GlobalizedMessage(CultureInfoProvider.English, "The event handler for '{0}' is not defined.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Eventhandleren til '{0}' er ikke defineret.")]
        EventHandlerNotDefined,

        [DefaultMessage("The account with the account number '{0}' could not be found.")]
        [GlobalizedMessage(CultureInfoProvider.English, "The account with the account number '{0}' could not be found.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke finde en konto med kontonummer: {0}")]
        AccountNotFound,

        [DefaultMessage("The budget account with the account number '{0}' could not be found.")]
        [GlobalizedMessage(CultureInfoProvider.English, "The budget account with the account number '{0}' could not be found.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke finde en budgetkonto med kontonummer: {0}")]
        BudgetAccountNotFound,

        [DefaultMessage("The address account with the account number '{0}' could not be found.")]
        [GlobalizedMessage(CultureInfoProvider.English, "The address account with the account number '{0}' could not be found.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke finde en adressekonto med kontonummer: {0}")]
        AddressAccountNotFound,

        [DefaultMessage("The accounting with the unique number '{0}' could not be found.")]
        [GlobalizedMessage(CultureInfoProvider.English, "The accounting with the unique number '{0}' could not be found.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Kunne ikke finde et regnskab med regnskabsnummer: {0}")]
        AccountingNotFound,

        [DefaultMessage("The XML schema for the locale data document is invalid.")]
        [GlobalizedMessage(CultureInfoProvider.English, "The XML schema for the locale data document is invalid.")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "XML-skemaet for det lokale data dokument er ugyldigt.")]
        LocaleDataSchemaIsInvalid,

        [DefaultMessage("The required attribute named '{0}' is missing or does not have a value.")]
        [GlobalizedMessage(CultureInfoProvider.English, "The required attribute named '{0}' is missing or does not have a value")]
        [GlobalizedMessage(CultureInfoProvider.Danish, "Attributten med navnet '{0}' mangler eller har ikke en værdi.")]
        MissingRequiredAttributeValue
    }
}