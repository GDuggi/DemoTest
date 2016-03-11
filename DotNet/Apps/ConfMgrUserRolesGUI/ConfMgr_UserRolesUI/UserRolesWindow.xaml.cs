using DBAccess.SqlServer;
using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace UserRolesUI_Admin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UserRolesWindow : Window
    {
        public UserRolesWindow()
        {
            InitializeComponent();
            RoleUtil.connString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            userRoles = new UserRoleDal(RoleUtil.connString);
        }

       UserRoleDal userRoles;
        private void userRolesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RoleDal roles = new RoleDal(RoleUtil.connString);
            RoleUtil.appRoles = roles.GetAll();
            RoleUtil.appRolesCodes = RoleUtil.appRoles.Select(s => s.RoleCode).ToList();
            //lstUnassignedRoles.ItemsSource = RoleUtil.AppRolesCodes;
            LoadUsersIntoUI();
        }

        private void LoadUsersIntoUI()
        {
            List<UserRoleView> users = userRoles.GetAllUsers();
            if(users !=null && users.Count > 0)
            {
                RoleUtil.AllUsers = users.Select(u => u.UserId).ToList();
                cmbUser.ItemsSource = RoleUtil.AllUsers;
            }
            else
            {
                lblMessage.Content = "There is no users in the database.";
                lblMessage.Foreground = new SolidColorBrush(Colors.Red);
            }

        }        

        private void btnAddRole_Click(object sender, RoutedEventArgs e)
        {
            lblMessage.Content = "";
            if(lstUnassignedRoles.SelectedItem != null)
            {
                string selectedItem = lstUnassignedRoles.SelectedItem.ToString();
                if (lstAssignedRoles.ItemsSource == null)
                    lstAssignedRoles.ItemsSource = new List<string>();

                RoleUtil.userAssignedRoles.Add(selectedItem);
                RoleUtil.userUnassignedRoles.Remove(selectedItem);
                if (!RoleUtil.deletedRoles.Contains(selectedItem))
                    RoleUtil.addedRoles.Add(selectedItem);
                RoleUtil.deletedRoles.Remove(selectedItem);
                RefreshRolesInUI();
            }
        }

        private void btnRemoveRole_Click(object sender, RoutedEventArgs e)
        {
            lblMessage.Content = "";
            if (lstAssignedRoles.SelectedItem != null)
            {
                string selectedItem = lstAssignedRoles.SelectedItem.ToString();
                if (lstUnassignedRoles.ItemsSource == null)
                    lstUnassignedRoles.ItemsSource = new List<string>();

                RoleUtil.userUnassignedRoles.Add(selectedItem);
                RoleUtil.userAssignedRoles.Remove(selectedItem);
                if (!RoleUtil.addedRoles.Contains(selectedItem))
                    RoleUtil.deletedRoles.Add(selectedItem);
                RoleUtil.addedRoles.Remove(selectedItem);
                RefreshRolesInUI();
            }
        }
        public void RefreshRolesInUI()
        {
            lstUnassignedRoles.ItemsSource = null;
            lstAssignedRoles.ItemsSource = null;
            lstUnassignedRoles.ItemsSource = new List<string>(RoleUtil.userUnassignedRoles);
            lstAssignedRoles.ItemsSource = new List<string>(RoleUtil.userAssignedRoles);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            lstUnassignedRoles.ItemsSource = null;
            lstAssignedRoles.ItemsSource = null;
            lblMessage.Content = "";
            cmbUser.Text="";
            RoleUtil.ClearAll();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            lblMessage.Content="";
            if(RoleUtil.addedRoles != null && RoleUtil.deletedRoles != null)
            {
                if(RoleUtil.addedRoles.Count==0 && RoleUtil.deletedRoles.Count==0)
                {
                    lblMessage.Content = "There is no updates.";
                    lblMessage.Foreground = (Brush)new BrushConverter().ConvertFromString("#00529B");
                    //lblMessage.Foreground = new SolidColorBrush("#9F6000");                    
                }
                else if (RoleUtil.addedRoles.Count > 0 || RoleUtil.deletedRoles.Count > 0)
                {
                    string msg = "";
                    try
                    {
                        string userId = cmbUser.Text;
                        
                        if (RoleUtil.addedRoles.Count > 0)
                        {
                            int rolesCnt = userRoles.AddRolesToUser(RoleUtil.addedRoles, userId);
                            msg = rolesCnt + " role(s) added to user : '" + userId + "' successfully.";                            
                            RoleUtil.addedRoles.Clear();
                        }
                        if(RoleUtil.deletedRoles.Count > 0)
                        {
                            int rolesCnt = userRoles.RemoveRolesFromUser(RoleUtil.deletedRoles, userId);
                            if (!string.IsNullOrEmpty(msg))
                                msg = msg + "\n";
                            msg =  msg + rolesCnt + " role(s) removed from user : '" + userId + "' successfully.";
                            RoleUtil.deletedRoles.Clear();
                        }
                        lblMessage.Content = msg;
                        lblMessage.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    catch (Exception ex)
                    {
                        if (!string.IsNullOrEmpty(msg))
                        {
                            lblMessage.Content = msg;
                            lblMessage.Foreground = new SolidColorBrush(Colors.Green);
                        }
                        MessageBox.Show(ex.Message,"Exception");
                    }
                }
            }
        }

        private void OnMouseEnterItem_UnassRoles(object sender, MouseEventArgs e)
        {
            ShowToolTip(sender, lstUnassignedRoles);
        }
        private void OnMouseEnterItem_AssRoles(object sender, MouseEventArgs e)
        {
            ShowToolTip(sender,lstAssignedRoles);
        }
        private void ShowToolTip(object sender, ListBox lstBox)
        {
            int currentindex;
            var result = sender as ListBoxItem;
            for (int i = 0; i < lstBox.Items.Count; i++)
            {
                if (lstBox.Items[i].ToString().Equals(result.Content.ToString()))
                {
                    currentindex = i;
                    result.ToolTip = RoleUtil.GetDesrForRoleCode(result.Content.ToString());
                    break;
                }
            }
        }
        bool isListsCleared = false;
        private void cmbUser_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!(RoleUtil.AllUsers.Contains((sender as ComboBox).Text)) || (sender as ComboBox).SelectedItem == null)
                {
                    lblMessage.Content = "Enter valid user ID.";
                    lblMessage.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                    GetUserDetails(sender, (sender as ComboBox).Text);
            }
            else
            {
                if (!isListsCleared)
                {
                    lstUnassignedRoles.ItemsSource = null;
                    lstAssignedRoles.ItemsSource = null;
                    lblMessage.Content = "";
                    RoleUtil.ClearAll();
                    isListsCleared = true;
                }
            }
        }
        private void cmbUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetUserDetails(sender, ((sender as ComboBox).SelectedItem != null) ? (sender as ComboBox).SelectedItem.ToString() : (sender as ComboBox).Text);
        }
        private void GetUserDetails(object sender,string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                isListsCleared = false;
                return;
            }
            else if (userRoles.IsUserIdValid(userID))
            {
                RoleUtil.GetUserRoles(userID);
                lstUnassignedRoles.ItemsSource = RoleUtil.userUnassignedRoles;
                lstAssignedRoles.ItemsSource = RoleUtil.userAssignedRoles;
                lblMessage.Content = "";
                isListsCleared = false;
            }
            else
            {
                lblMessage.Content = "User id: '" + (sender as ComboBox).Text + "' does not exist in the database.";
                lblMessage.Foreground = new SolidColorBrush(Colors.Red);
                isListsCleared = false;
            }
        }
        


    }
}
