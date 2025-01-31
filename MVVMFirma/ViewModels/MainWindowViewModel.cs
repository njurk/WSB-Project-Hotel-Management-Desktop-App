using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;

namespace MVVMFirma.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        private ReadOnlyCollection<CommandViewModel> _Commands;
        private ObservableCollection<WorkspaceViewModel> _Workspaces;
        private BaseCommand _openRaportPrzychodowCommand;
        private BaseCommand _openRaportOdwiedzinCommand;
        private BaseCommand _openAnalizaPracownikowCommand;
        #endregion

        #region Commands
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_Commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _Commands;
            }
        }

        // komendy odpowiadające za otwieranie widoków tworzących i dodających z listy bocznej
        private List<CommandViewModel> CreateCommands()
        {
            Messenger.Default.Register<string>(this, open);
            // open - metoda zdefiniowana w Helpers

            return new List<CommandViewModel>
            {
                /*new CommandViewModel(
                    "Cennik",
                    new BaseCommand(() => this.CreateView(new NowyCennikViewModel()))),*/

                new CommandViewModel(
                    "Cenniki",
                    new BaseCommand(() => this.ShowAllView(new WszystkieCennikViewModel()))),

                /*new CommandViewModel(
                    "Faktura",
                    new BaseCommand(() => this.CreateView(new NowyFakturaViewModel()))),*/

                new CommandViewModel(
                    "Faktury",
                    new BaseCommand(() => this.ShowAllView(new WszystkieFakturaViewModel()))),

                /*new CommandViewModel(
                    "Klasa pokoju",
                    new BaseCommand(() => this.CreateView(new NowyKlasaPokojuViewModel()))),*/

                new CommandViewModel(
                    "Klasy pokojów",
                    new BaseCommand(() => this.ShowAllView(new WszystkieKlasaPokojuViewModel()))),

                /*new CommandViewModel(
                    "Klient",
                    new BaseCommand(() => this.CreateView(new NowyKlientViewModel()))),*/

                new CommandViewModel(
                    "Klienci",
                    new BaseCommand(() => this.ShowAllView(new WszystkieKlientViewModel()))),

                /*new CommandViewModel(
                    "Kraj",
                    new BaseCommand(() => this.CreateView(new NowyKrajViewModel()))),*/

                new CommandViewModel(
                    "Kraje",
                    new BaseCommand(() => this.ShowAllView(new WszystkieKrajViewModel()))),

                /*new CommandViewModel(
                    "Płatność",
                    new BaseCommand(() => this.CreateView(new NowyPlatnoscViewModel()))),*/

                new CommandViewModel(
                    "Płatności",
                    new BaseCommand(() => this.ShowAllView(new WszystkiePlatnoscViewModel()))),

                /*new CommandViewModel(
                    "Pokój",
                    new BaseCommand(() => this.CreateView(new NowyPokojViewModel()))),*/

                new CommandViewModel(
                    "Pokoje",
                    new BaseCommand(() => this.ShowAllView(new WszystkiePokojViewModel()))),

                /*new CommandViewModel(
                    "Pracownik",
                    new BaseCommand(() => this.CreateView(new NowyPracownikViewModel()))),*/

                new CommandViewModel(
                    "Pracownicy",
                    new BaseCommand(() => this.ShowAllView(new WszystkiePracownikViewModel()))),

                /*new CommandViewModel(
                    "Rezerwacja",
                    new BaseCommand(() => this.CreateView(new NowyRezerwacjaViewModel()))),*/

                new CommandViewModel(
                    "Rezerwacje",
                    new BaseCommand(() => this.ShowAllView(new WszystkieRezerwacjaViewModel()))),

                /*new CommandViewModel(
                    "Stanowisko",
                    new BaseCommand(() => this.CreateView(new NowyStanowiskoViewModel()))),*/

                new CommandViewModel(
                    "Stanowiska",
                    new BaseCommand(() => this.ShowAllView(new WszystkieStanowiskoViewModel()))),

                /*new CommandViewModel(
                    "Sposób płatności",
                    new BaseCommand(() => this.CreateView(new NowySposobPlatnosciViewModel()))),*/

                new CommandViewModel(
                    "Sposoby płatności",
                    new BaseCommand(() => this.ShowAllView(new WszystkieSposobPlatnosciViewModel()))),

                /*new CommandViewModel(
                    "Status płatności",
                    new BaseCommand(() => this.CreateView(new NowyStatusPlatnosciViewModel()))),*/

                new CommandViewModel(
                    "Statusy płatności",
                    new BaseCommand(() => this.ShowAllView(new WszystkieStatusPlatnosciViewModel()))),

                /*new CommandViewModel(
                    "Typ pokoju",
                    new BaseCommand(() => this.CreateView(new NowyTypPokojuViewModel()))),*/

                new CommandViewModel(
                    "Typy pokojów",
                    new BaseCommand(() => this.ShowAllView(new WszystkieTypPokojuViewModel()))),

                /*new CommandViewModel(
                    "Stawka VAT",
                    new BaseCommand(() => this.CreateView(new NowyVATViewModel()))),*/

                new CommandViewModel(
                    "Stawki VAT",
                    new BaseCommand(() => this.ShowAllView(new WszystkieVATViewModel()))),

                 /*new CommandViewModel(
                    "Zniżka",
                    new BaseCommand(() => this.CreateView(new NowyZnizkaViewModel()))),*/

                new CommandViewModel(
                    "Zniżki",
                    new BaseCommand(() => this.ShowAllView(new WszystkieZnizkaViewModel()))),

                /*new CommandViewModel(
                    "Obecność",
                    new BaseCommand(() => this.CreateView(new NowyObecnoscViewModel()))),*/

                new CommandViewModel(
                    "Obecności",
                    new BaseCommand(() => this.ShowAllView(new WszystkieObecnoscViewModel())))
            };
        }
        #endregion

        #region Workspaces
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_Workspaces == null)
                {
                    _Workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _Workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _Workspaces;
            }
        }
        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }
        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            this.Workspaces.Remove(workspace);
        }
        #endregion

        #region Private Helpers
        // uniwersalne metody wyświetlania widoków Nowy, Edycji oraz Wszystkie 
        private void CreateView(WorkspaceViewModel nowy)
        {
            this.Workspaces.Add(nowy);
            this.SetActiveWorkspace(nowy);
        }
        private void EditView(WorkspaceViewModel nowy)
        {
            this.Workspaces.Add(nowy);
            this.SetActiveWorkspace(nowy);
        }
        private void ShowAllView(WorkspaceViewModel nowy)
        {
            var workspace = this.Workspaces.FirstOrDefault(vm => vm.GetType() == nowy.GetType());

            if (workspace == null)
            {
                this.Workspaces.Add(nowy);
                workspace = nowy;
            }

            this.SetActiveWorkspace(workspace);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        private void open(string name) // name - wysyłany komunikat
        {
            // dodawanie
            if (name.EndsWith("Add"))
            {
                if (name == "CennikiAdd")
                    CreateView(new NowyCennikViewModel());
                if (name == "FakturyAdd")
                    CreateView(new NowyFakturaViewModel());
                if (name == "Klasy pokojówAdd")
                    CreateView(new NowyKlasaPokojuViewModel());
                if (name == "KlienciAdd")
                    CreateView(new NowyKlientViewModel());
                if (name == "PłatnościAdd")
                    CreateView(new NowyPlatnoscViewModel());
                if (name == "PokojeAdd")
                    CreateView(new NowyPokojViewModel());
                if (name == "PracownicyAdd")
                    CreateView(new NowyPracownikViewModel());
                if (name == "RezerwacjeAdd")
                    CreateView(new NowyRezerwacjaViewModel());
                if (name == "StanowiskaAdd")
                    CreateView(new NowyStanowiskoViewModel());
                if (name == "Sposoby płatnościAdd")
                    CreateView(new NowySposobPlatnosciViewModel());
                if (name == "Statusy płatnościAdd")
                    CreateView(new NowyStatusPlatnosciViewModel());
                if (name == "Typy pokojówAdd")
                    CreateView(new NowyTypPokojuViewModel());
                if (name == "KrajeAdd")
                    CreateView(new NowyKrajViewModel());
                if (name == "Stawki VATAdd")
                    CreateView(new NowyVATViewModel());
                if (name == "ZniżkiAdd")
                    CreateView(new NowyZnizkaViewModel());
                if (name == "ObecnościAdd")
                    CreateView(new NowyObecnoscViewModel());
            }
            // edycja - z komunikatu wyodrębniane jest ID a następnie wysyłane do specjalnego konstruktora klasy Nowy..ViewModel
            else if (name.Contains("Edit-"))
            {
                string[] splitMessage = name.Split('-');
                string splitName = splitMessage[0];
                string splitId = splitMessage[1];

                if (int.TryParse(splitMessage[1], out int itemId))
                {
                    if (splitName == "CennikiEdit")
                        EditView(new NowyCennikViewModel(itemId));
                    if (splitName == "FakturyEdit")
                        EditView(new NowyFakturaViewModel(itemId));
                    if (splitName == "Klasy pokojówEdit")
                        EditView(new NowyKlasaPokojuViewModel(itemId));
                    if (splitName == "KlienciEdit")
                        EditView(new NowyKlientViewModel(itemId));
                    if (splitName == "PłatnościEdit")
                        EditView(new NowyPlatnoscViewModel(itemId));
                    if (splitName == "PokojeEdit")
                        EditView(new NowyPokojViewModel(itemId));
                    if (splitName == "PracownicyEdit")
                        EditView(new NowyPracownikViewModel(itemId));
                    if (splitName == "RezerwacjeEdit")
                        EditView(new NowyRezerwacjaViewModel(itemId));
                    if (splitName == "StanowiskaEdit")
                        EditView(new NowyStanowiskoViewModel(itemId));
                    if (splitName == "Sposoby płatnościEdit")
                        EditView(new NowySposobPlatnosciViewModel(itemId));
                    if (splitName == "Statusy płatnościEdit")
                        EditView(new NowyStatusPlatnosciViewModel(itemId));
                    if (splitName == "Typy pokojówEdit")
                        EditView(new NowyTypPokojuViewModel(itemId));
                    if (splitName == "KrajeEdit")
                        EditView(new NowyKrajViewModel(itemId));
                    if (splitName == "Stawki VATEdit")
                        EditView(new NowyVATViewModel(itemId));
                    if (splitName == "ZniżkiEdit")
                        EditView(new NowyZnizkaViewModel(itemId));
                    if (splitName == "ObecnościEdit")
                        EditView(new NowyObecnoscViewModel(itemId));
                }
            }
        }
        #endregion

        #region Business Views Commands
        public BaseCommand OpenRaportPrzychodowCommand
        {
            get
            {
                if (_openRaportPrzychodowCommand == null)
                {
                    _openRaportPrzychodowCommand = new BaseCommand(OpenRaportPrzychodow);
                }
                return _openRaportPrzychodowCommand;
            }
        }

        public BaseCommand OpenRaportOdwiedzinCommand
        {
            get
            {
                if (_openRaportOdwiedzinCommand == null)
                {
                    _openRaportOdwiedzinCommand = new BaseCommand(OpenRaportOdwiedzin);
                }
                return _openRaportOdwiedzinCommand;
            }
        }

        public BaseCommand OpenAnalizaPracownikowCommand
        {
            get
            {
                if (_openAnalizaPracownikowCommand == null)
                {
                    _openAnalizaPracownikowCommand = new BaseCommand(OpenAnalizaPracownikow);
                }
                return _openAnalizaPracownikowCommand;
            }
        }
        #endregion

        #region Business Views Methods
        private void OpenRaportPrzychodow()
        {
            WorkspaceViewModel workspace = new RaportPrzychodowViewModel();

            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }
        private void OpenRaportOdwiedzin()
        {
            WorkspaceViewModel workspace = new RaportOdwiedzinViewModel();

            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        private void OpenAnalizaPracownikow()
        {
            WorkspaceViewModel workspace = new AnalizaPracownikowViewModel();

            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }
        #endregion
    }
}
