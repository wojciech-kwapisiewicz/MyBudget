﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyBudget.OperationsLoading.Tests.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class TestFiles {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TestFiles() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MyBudget.OperationsLoading.Tests.Resources.TestFiles", typeof(TestFiles).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data operacji;Kwota transakcji;Opis;Saldo po operacji
        ///2016-10-01;1000.12;PRZELEW UZNANIOWY (NADANO 01-10-2016) ABC WYPŁATA 01 10 2016    ABC. Z O.O.   UL.ABC 1 11-111 WARSZAWA  01 2345 6789 0123 4567 8901 2345 ABC CR/Aaaa ;1000.12
        ///2016-10-02;-100.56;PRZELEW NA RACHUNEK NUMER 01 2345 6789 0123 4567 8901 2346 Abc ;899.56
        ///2016-10-03;-4.00;PRZELEW OBCIĄŻENIOWY asadfasfdsaf   01 2345 6789 0123 4567 8901 2347 Abc 1 Asadfasdf ;895.56
        ///2016-10-05;-10.11;OPERACJA KARTĄ ZLOTA 123456XXXXXX7890 000001 WYPL ATA GOTÓW [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string BGZParser_LongDescPayment {
            get {
                return ResourceManager.GetString("BGZParser_LongDescPayment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data operacji;Kwota transakcji;Opis;Saldo po operacji
        ///2016-10-01;1000.12;PRZELEW UZNANIOWY (NADANO 01-10-2016) ABC WYPŁATA 01 10 2016    ABC. Z O.O.   UL.ABC 1 11-111 WARSZAWA  01 2345 6789 0123 4567 8901 2345 ABC CR/Aaaa ;1000.12
        ///2016-10-02;-100.56;PRZELEW NA RACHUNEK NUMER 01 2345 6789 0123 4567 8901 2346 Abc ;899.56
        ///2016-10-03;-4.00;PRZELEW OBCIĄŻENIOWY asadfasfdsaf   01 2345 6789 0123 4567 8901 2347 Abc 1 Asadfasdf ;895.56
        ///2016-10-05;-10.11;OPERACJA KARTĄ ZLOTA 123456XXXXXX7890 000001 WYPL ATA GOTÓW [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string BGZParser_StandardCases {
            get {
                return ResourceManager.GetString("BGZParser_StandardCases", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Numer rachunku/karty,Data transakcji,Data rozliczenia,Rodzaj transakcji,Na konto/Z konta,Odbiorca/Zleceniodawca,Opis,Obciążenia,Uznania,Saldo,Waluta
        ///&quot;PL00 0123 4567 8910 0000 1111 1234&quot;,&quot;2014-09-20&quot;,&quot;2014-09-20&quot;,&quot;PRZELEW WEWNĘTRZNY WYCHODZĄCY&quot;,&quot;&quot;,&quot;AAA BBB&quot;,&quot;Przelew własny&quot;,&quot;-1234.00&quot;,&quot;&quot;,&quot;10000.00&quot;,&quot;PLN&quot;
        ///&quot;PL00 0123 4567 8910 0000 1111 1234&quot;,&quot;2014-09-19&quot;,&quot;2014-09-19&quot;,&quot;OBCIĄŻENIE&quot;,&quot;&quot;,&quot;&quot;,&quot;PODATEK OD ODSETEK&quot;,&quot;-11.22&quot;,&quot;&quot;,&quot;0.00&quot;,&quot;PLN&quot;
        ///&quot;PL00 0123 4567 8910 0000 1111 1234&quot;,&quot;2014-09-18&quot;,&quot;2014-09-18&quot;,&quot;UZNANIE&quot;,&quot;&quot;, [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MilleniumParser_Sample {
            get {
                return ResourceManager.GetString("MilleniumParser_Sample", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Numer rachunku/karty,Data transakcji,Data rozliczenia,Rodzaj transakcji,Na konto/Z konta,Odbiorca/Zleceniodawca,Opis,Obciążenia,Uznania,Saldo,Waluta
        ///&quot;PL00 0123 4567 8910 0000 1111 1234&quot;,&quot;2014-09-17&quot;,&quot;2014-09-17&quot;,&quot;PRZELEW PRZYCHODZĄCY&quot;,&quot;03 10 2039 5800 0012 3456 7890 00&quot;,&quot;AAA BBB&quot;,&quot;Tytul&quot;,&quot;&quot;,&quot;123.45&quot;,&quot;0.00&quot;,&quot;PLN&quot;
        ///.
        /// </summary>
        internal static string MilleniumParser_Sample1Entry {
            get {
                return ResourceManager.GetString("MilleniumParser_Sample1Entry", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [logo iPKO]
        ///Klient: XYZ | Numer klienta: 1111111 | Czas do końca sesji: 09:45s odśwież
        ///Ostatnie logowanie: udane 2000-01-01 10:00:00 | nieudane 2000-01-01 10:00:00
        ///Rachunki
        ///Transakcje
        ///Karty
        ///IKO
        ///Fundusze
        ///Usługi 
        ///maklerskie
        ///Ubezpieczenia
        ///Doładowania
        ///Dostęp
        ///Kontakt
        ///Poczta 
        ///iPKO
        ///Oferta 
        ///dla Ciebie
        ///Wyloguj
        ///Karty debetowe
        ///Karty kredytowe
        ///Szczegóły karty kredytowej
        ///Operacje bieżące
        ///Zestawienia operacji
        ///Przelew z karty
        ///Spłać kartę
        ///Karty obciążeniowe
        ///Karty przedpłacone
        ///
        /// [logo iPKO] [log [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PkoBpCreditCardParser_Sample {
            get {
                return ResourceManager.GetString("PkoBpCreditCardParser_Sample", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [logo iPKO]
        ///Klient: XYZ | Numer klienta: 1111111 | Czas do końca sesji: 09:45s odśwież
        ///Ostatnie logowanie: udane 2000-01-01 10:00:00 | nieudane 2000-01-01 10:00:00
        ///Rachunki
        ///Transakcje
        ///Karty
        ///IKO
        ///Fundusze
        ///Usługi 
        ///maklerskie
        ///Ubezpieczenia
        ///Doładowania
        ///Dostęp
        ///Kontakt
        ///Poczta 
        ///iPKO
        ///Oferta 
        ///dla Ciebie
        ///Wyloguj
        ///Karty debetowe
        ///Karty kredytowe
        ///Szczegóły karty kredytowej
        ///Operacje bieżące
        ///Zestawienia operacji
        ///Przelew z karty
        ///Spłać kartę
        ///Karty obciążeniowe
        ///Karty przedpłacone
        ///
        /// [logo iPKO] [log [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PkoBpCreditCardParser_Sample1 {
            get {
                return ResourceManager.GetString("PkoBpCreditCardParser_Sample1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;iso-8859-2&quot;?&gt;
        ///&lt;account-history&gt;
        ///  &lt;search&gt;
        ///    &lt;account&gt;03102039580000123456789000&lt;/account&gt;
        ///    &lt;date since=&apos;2013-02-01&apos; to=&apos;2012-02-20&apos;/&gt;
        ///    &lt;filtering&gt;Wszystkie&lt;/filtering&gt;
        ///  &lt;/search&gt;
        ///  &lt;operations&gt;
        ///    &lt;operation&gt;
        ///      &lt;exec-date&gt;2013-02-02&lt;/exec-date&gt;
        ///      &lt;order-date&gt;2013-02-02&lt;/order-date&gt;
        ///      &lt;type&gt;Przelew z rachunku&lt;/type&gt;
        ///      &lt;description&gt;
        ///        Nr rach. przeciwst.: 11 2222 3333 4444 5555 6666 7777
        ///        Dane adr. rach. przeciwst.: Name
        ///     [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PkoBpParser_1Entry {
            get {
                return ResourceManager.GetString("PkoBpParser_1Entry", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;iso-8859-2&quot;?&gt;
        ///&lt;account-history&gt;
        ///  &lt;search&gt;
        ///    &lt;account&gt;03102039580000123456789000&lt;/account&gt;
        ///    &lt;date since=&apos;2013-02-01&apos; to=&apos;2012-02-20&apos;/&gt;
        ///    &lt;filtering&gt;Wszystkie&lt;/filtering&gt;
        ///  &lt;/search&gt;
        ///  &lt;operations&gt;
        ///    &lt;operation&gt;
        ///      &lt;exec-date&gt;2013-02-03&lt;/exec-date&gt;
        ///      &lt;order-date&gt;2013-02-03&lt;/order-date&gt;
        ///      &lt;type&gt;Wypłata z bankomatu&lt;/type&gt;
        ///      &lt;description&gt;Tytuł: 12345678901234567890123
        ///Lokalizacja: Kraj: POLSKA Miasto: CityOfSth Adres: U AAA 222
        ///Data i czas oper [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PkoBpParser_ZerosInTitle {
            get {
                return ResourceManager.GetString("PkoBpParser_ZerosInTitle", resourceCulture);
            }
        }
    }
}
