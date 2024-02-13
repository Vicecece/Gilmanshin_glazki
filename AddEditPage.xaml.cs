using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gilmanshin_glazki
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Agent currentAgent = new Agent();
        public bool IsServiceExist = false;
        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();
            if (SelectedAgent != null)
            {
                IsServiceExist = true;
                currentAgent = SelectedAgent;
            }
            else
            {
                SaleBtn.Visibility = Visibility.Hidden;
            }

            DataContext = currentAgent;
        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                currentAgent.Logo = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentAgent.Title))
                errors.AppendLine("Укажите наименование агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Address))
                errors.AppendLine("Укажите адрес агента");
            if (string.IsNullOrWhiteSpace(currentAgent.DirectorName))
                errors.AppendLine("Укажите ФИО директора агента");
            if (ComboType.SelectedItem == null)
                errors.AppendLine("Укажите тип агента");
            if (currentAgent.Priority <= 0)
                errors.AppendLine("Укажите положительный приоритет агента");
            if (string.IsNullOrWhiteSpace(currentAgent.INN))
                errors.AppendLine("Укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(currentAgent.KPP))
                errors.AppendLine("Укажите КПП агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Phone))
                errors.AppendLine("Укажите телефон агента");
            else
            {
                string ph = currentAgent.Phone.Replace("(", "").Replace("-", "").Replace("+", "");
                if (((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11) ||
                    (ph[1] == '3' && ph.Length != 12))errors.AppendLine("Укажите корректный номер агента");
            }
            if (string.IsNullOrWhiteSpace(currentAgent.Email))
                errors.AppendLine("Укажите почту агента");
            else if (!EmailValidator(currentAgent.Email))
            {
                errors.AppendLine("Почта в неправильном формате");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            currentAgent.AgentTypeID = ComboType.SelectedIndex + 1;


            var allServices = Gilmanshin_GlazkiEntities.GetContext().Agent.ToList();
            allServices = allServices.Where(p => p.Title == currentAgent.Title).ToList();
            if (allServices.Count == 0 || IsServiceExist == true)
            {
                if (currentAgent.ID == 0)
                    Gilmanshin_GlazkiEntities.GetContext().Agent.Add(currentAgent);

                try
                {
                    Gilmanshin_GlazkiEntities.GetContext().SaveChanges();
                    MessageBox.Show("информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                Gilmanshin_GlazkiEntities.GetContext().SaveChanges();
                MessageBox.Show("Уже существует такая услуга");
            }
        }
        private bool EmailValidator(string email)
        {
            return email.Contains("@") && email.Contains('.');
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentAgent = (sender as Button)?.DataContext as Agent;

            var currentProducts = Gilmanshin_GlazkiEntities.GetContext().ProductSale.ToList();
            currentProducts = currentProducts.Where(p => p.AgentID == currentAgent.ID).ToList();
            if (currentProducts.Count != 0)
            {
                MessageBox.Show("Невозможно выполнить удаления, так как существуют записи на эту услугу");
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?",
                        "Внимание!",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    try
                    {
                        Gilmanshin_GlazkiEntities.GetContext().Agent.Remove(currentAgent);
                        Gilmanshin_GlazkiEntities.GetContext().SaveChanges();
                        Manager.MainFrame.GoBack();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }
            }
        }

        private void SaleBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AgentSalePage(currentAgent));
        }
    }
}

