using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Filmtipset.Models;
using MediaPortal.GUI.Library;

namespace Filmtipset
{
    public partial class Configuration : Form
    {
        public Configuration()
        {
            InitializeComponent();

            FilmtipsetSettings.LoadSettings();
            tbApiKey.Text = string.Empty;
            tbUserId.Text = string.Empty;
            foreach (Account account in FilmtipsetSettings.Accounts)
            {
                clbAccounts.Items.Add(account.Name, false);
            }

            cbFanart.Checked = FilmtipsetSettings.EnableFanart;
            if (FilmtipsetSettings.EnableFanart)
            {
                cbPreferFanartPoster.Enabled = true;
                cbPreferFanartPoster.Checked = FilmtipsetSettings.PreferFanartPoster;
            }
            else
            {
                cbPreferFanartPoster.Checked = false;
                cbPreferFanartPoster.Enabled = false;
            }
            tbFanartApiKey.Text = FilmtipsetSettings.PersonalFanartAPIKey;

            cbViaplay.Checked = FilmtipsetSettings.EnableViaplay;
            cbDreamfilm.Checked = FilmtipsetSettings.EnableDreamfilm;
            cbSwefilmer.Checked = FilmtipsetSettings.EnableSwefilmer;
            cbMovHunter.Checked = FilmtipsetSettings.EnableMovHunter;
            cbTfPlay.Checked = FilmtipsetSettings.EnableTfPlay;
            cbSweflix.Checked = FilmtipsetSettings.EnableSweflix;
            tbExtraSites.Text = FilmtipsetSettings.OnlineVideosExtraSites;

            cbTvWishListEmail.Checked = FilmtipsetSettings.TvWishListEmail;

            cbTvWishSearchLogic.Items.Add(new TvWishesSearchSettingItem(TvWishesSearchSetting.title_TitleOrOrgTitle, "TV title equals movie title or movie org.title"));
            cbTvWishSearchLogic.Items.Add(new TvWishesSearchSettingItem(TvWishesSearchSetting.title_TitleOrOrgTitle_And_Description_Year, "TV title equals movie title or movie org.title. And the description contains movie year"));
            cbTvWishSearchLogic.Items.Add(new TvWishesSearchSettingItem(TvWishesSearchSetting.title_Title_Or_Description_OrgTitleAndYear, "TV title equals movie title. Or the description contains movie org.title and movie year."));
            if (Enum.IsDefined(typeof(TvWishesSearchSetting), (int)FilmtipsetSettings.TvWishSearchLogic))
                cbTvWishSearchLogic.SelectedIndex = (int)FilmtipsetSettings.TvWishSearchLogic;
            else
                cbTvWishSearchLogic.SelectedIndex = 0;
            FilmtipsetSettings.TvWishSearchLogic = cbTvWishSearchLogic.SelectedIndex;
        }


        private void llRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://www.filmtipset.se/");
        }

        private void llApiKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://www.filmtipset.se/api.cgi");
        }

        private void llUserId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://www.filmtipset.se/yourpage.cgi");
        }

        private void llFanartApiKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://fanart.tv/get-an-api-key/");
        }

        private void cbViaplay_CheckedChanged(object sender, EventArgs e)
        {
            FilmtipsetSettings.EnableViaplay = cbViaplay.Checked;
        }

        private void cbDreamfilm_CheckedChanged(object sender, EventArgs e)
        {
            FilmtipsetSettings.EnableDreamfilm = cbDreamfilm.Checked;
        }

        private void cbSwefilmer_CheckedChanged(object sender, EventArgs e)
        {
            FilmtipsetSettings.EnableSwefilmer = cbSwefilmer.Checked;
        }

        private void cbMovHunter_CheckedChanged(object sender, EventArgs e)
        {
            FilmtipsetSettings.EnableMovHunter = cbMovHunter.Checked;
        }

        private void cbTfPlay_CheckedChanged(object sender, EventArgs e)
        {
            FilmtipsetSettings.EnableTfPlay = cbTfPlay.Checked;
        }

        private void cbSweflix_CheckedChanged(object sender, EventArgs e)
        {
            FilmtipsetSettings.EnableSweflix = cbSweflix.Checked;
        }

        private void tbExtraSites_TextChanged(object sender, EventArgs e)
        {
            FilmtipsetSettings.OnlineVideosExtraSites = tbExtraSites.Text;
        }

        private void tbFanartApiKey_TextChanged(object sender, EventArgs e)
        {
            FilmtipsetSettings.PersonalFanartAPIKey = tbFanartApiKey.Text;
        }
        private bool _tempPreferFanartPoster = false;
        private void cbFanart_CheckedChanged(object sender, EventArgs e)
        {
            FilmtipsetSettings.EnableFanart = cbFanart.Checked;
            if (!cbFanart.Checked)
            {
                _tempPreferFanartPoster = FilmtipsetSettings.PreferFanartPoster;
                cbPreferFanartPoster.Checked = false;
                cbPreferFanartPoster.Enabled = false;
                FilmtipsetSettings.PreferFanartPoster = false;
            }
            else
            {
                cbPreferFanartPoster.Enabled = true;
                cbPreferFanartPoster.Checked = _tempPreferFanartPoster;
            }
        }

        private void cbPreferFanartPoster_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbFanart.Checked)
            {
                cbPreferFanartPoster.Checked = false;
                cbPreferFanartPoster.Enabled = false;
                FilmtipsetSettings.PreferFanartPoster = false;
            }
            else
            {
                cbPreferFanartPoster.Enabled = true;
                FilmtipsetSettings.PreferFanartPoster = cbPreferFanartPoster.Checked;
            }

        }

        private void cbTvWishListEmail_CheckedChanged(object sender, EventArgs e)
        {
            FilmtipsetSettings.TvWishListEmail = cbTvWishListEmail.Checked;
        }

        private void cbTvWishSearchLogic_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilmtipsetSettings.TvWishSearchLogic = cbTvWishSearchLogic.SelectedIndex;
        }

        private void bUpdateUser_Click(object sender, EventArgs e)
        {
            bUpdateUser.Enabled = false;
            tbApiKey.Enabled = false;
            tbUserId.Enabled = false;
            bOK.Enabled = false;
            bRemoveAccounts.Enabled = false;
            int id = 0;
            string idTxt = tbUserId.Text.Trim();
            int.TryParse(idTxt, out id);
            if (!string.IsNullOrEmpty(tbApiKey.Text.Trim()) || id > 0)
            {
                Account account = Util.Helpers.GetDefaultUser();
                account.ApiKey = tbApiKey.Text;
                account.Id = id;
                account = Filmtipset.API.FilmtipsetAPI.Instance.GetAccount(account);
                if (!string.IsNullOrEmpty(account.Name) && !clbAccounts.Items.Contains(account.Name))
                {
                    clbAccounts.Items.Add(account.Name, false);
                    FilmtipsetSettings.Accounts.Add(account);
                    tbApiKey.Text = "";
                    tbUserId.Text = "";
                    MessageBox.Show(string.Format("Added the {0} account.", account.Name), "Account added");
                }
                else if (!string.IsNullOrEmpty(account.Name) && clbAccounts.Items.Contains(account.Name))
                {
                    MessageBox.Show(string.Format("The {0} account is already added.\n\rIf you would like to update it please remove the old one first.", account.Name), "Account added");
                }
                else
                {
                    Log.Debug(string.Format("[Filmtipset] Could not add account. Key: {0}, Id: {1} ", tbApiKey, id));
                    MessageBox.Show("Could not add the account!", "Error");
                }
            }
            else if (!string.IsNullOrEmpty(idTxt))
            {
                MessageBox.Show("User ID must be a number!", "Error");
            }

            bUpdateUser.Enabled = true;
            tbApiKey.Enabled = true;
            tbUserId.Enabled = true;
            bOK.Enabled = true;
            bRemoveAccounts.Enabled = true;
        }

        private void bRemoveAccounts_Click(object sender, EventArgs e)
        {
            List<string> checkedItems = clbAccounts.CheckedItems.Cast<string>().ToList<string>();
            if (checkedItems.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show(string.Format("Do you wish to remove {0}?", string.Join(",", checkedItems.ToArray())),
                    "Remove Account(s)",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    foreach (string accountName in checkedItems)
                    {
                    clbAccounts.Items.Remove(accountName);
                    Account account = FilmtipsetSettings.Accounts.FirstOrDefault(a => a.Name == accountName);
                    if (account != null)
                        FilmtipsetSettings.Accounts.Remove(account);
                    if (accountName == FilmtipsetSettings.CurrentAccount.Name)
                        FilmtipsetSettings.CurrentAccount = new Account() { Name = "", ApiKey = "", Id = 0 };
                    }
                }
            }
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            int id = 0;
            int.TryParse(tbUserId.Text, out id);
            if (id > 0 || !string.IsNullOrEmpty(tbApiKey.Text.Trim()))
            {
                DialogResult dialogResult = MessageBox.Show("The account information have not been added.\n\rDo you wish to close the Filmtipset settings without adding the account?", "Add Account", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    FilmtipsetSettings.SaveSettings();
                    this.Close();
                }
            }
            else
            {
                FilmtipsetSettings.SaveSettings();
                this.Close();
            }
        }

        private void Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
        }



    }
}