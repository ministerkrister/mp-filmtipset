namespace Filmtipset
{
    partial class Configuration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lApiKey = new System.Windows.Forms.Label();
            this.tbApiKey = new System.Windows.Forms.TextBox();
            this.llApiKey = new System.Windows.Forms.LinkLabel();
            this.llRegister = new System.Windows.Forms.LinkLabel();
            this.cbFanart = new System.Windows.Forms.CheckBox();
            this.lUserId = new System.Windows.Forms.Label();
            this.tbUserId = new System.Windows.Forms.TextBox();
            this.llUserId = new System.Windows.Forms.LinkLabel();
            this.gbAccount = new System.Windows.Forms.GroupBox();
            this.bUpdateUser = new System.Windows.Forms.Button();
            this.gbFanart = new System.Windows.Forms.GroupBox();
            this.lFanartApiKey = new System.Windows.Forms.Label();
            this.cbPreferFanartPoster = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbOnlineVideos = new System.Windows.Forms.GroupBox();
            this.lExtraSites = new System.Windows.Forms.Label();
            this.tbExtraSites = new System.Windows.Forms.TextBox();
            this.cbMovHunter = new System.Windows.Forms.CheckBox();
            this.cbTfPlay = new System.Windows.Forms.CheckBox();
            this.cbSweflix = new System.Windows.Forms.CheckBox();
            this.cbSwefilmer = new System.Windows.Forms.CheckBox();
            this.cbDreamfilm = new System.Windows.Forms.CheckBox();
            this.cbViaplay = new System.Windows.Forms.CheckBox();
            this.bOK = new System.Windows.Forms.Button();
            this.clbAccounts = new System.Windows.Forms.CheckedListBox();
            this.gbAccounts = new System.Windows.Forms.GroupBox();
            this.bRemoveAccounts = new System.Windows.Forms.Button();
            this.gbTvWishList = new System.Windows.Forms.GroupBox();
            this.lTvWishSearchLogic = new System.Windows.Forms.Label();
            this.cbTvWishSearchLogic = new System.Windows.Forms.ComboBox();
            this.cbTvWishListEmail = new System.Windows.Forms.CheckBox();
            this.tbFanartApiKey = new System.Windows.Forms.TextBox();
            this.llFanartApiKey = new System.Windows.Forms.LinkLabel();
            this.gbAccount.SuspendLayout();
            this.gbFanart.SuspendLayout();
            this.gbOnlineVideos.SuspendLayout();
            this.gbAccounts.SuspendLayout();
            this.gbTvWishList.SuspendLayout();
            this.SuspendLayout();
            // 
            // lApiKey
            // 
            this.lApiKey.AutoSize = true;
            this.lApiKey.Location = new System.Drawing.Point(6, 20);
            this.lApiKey.Name = "lApiKey";
            this.lApiKey.Size = new System.Drawing.Size(45, 13);
            this.lApiKey.TabIndex = 0;
            this.lApiKey.Text = "&API Key";
            // 
            // tbApiKey
            // 
            this.tbApiKey.Location = new System.Drawing.Point(6, 38);
            this.tbApiKey.Name = "tbApiKey";
            this.tbApiKey.Size = new System.Drawing.Size(207, 20);
            this.tbApiKey.TabIndex = 1;
            // 
            // llApiKey
            // 
            this.llApiKey.AutoSize = true;
            this.llApiKey.Location = new System.Drawing.Point(6, 61);
            this.llApiKey.Name = "llApiKey";
            this.llApiKey.Size = new System.Drawing.Size(79, 13);
            this.llApiKey.TabIndex = 2;
            this.llApiKey.TabStop = true;
            this.llApiKey.Text = "Get an API key";
            this.llApiKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llApiKey_LinkClicked);
            // 
            // llRegister
            // 
            this.llRegister.AutoSize = true;
            this.llRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llRegister.Location = new System.Drawing.Point(14, 452);
            this.llRegister.Name = "llRegister";
            this.llRegister.Size = new System.Drawing.Size(252, 20);
            this.llRegister.TabIndex = 3;
            this.llRegister.TabStop = true;
            this.llRegister.Text = "Register at http://www.filmtipset.se";
            this.llRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llRegister_LinkClicked);
            // 
            // cbFanart
            // 
            this.cbFanart.AutoSize = true;
            this.cbFanart.Checked = true;
            this.cbFanart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFanart.Location = new System.Drawing.Point(9, 20);
            this.cbFanart.Name = "cbFanart";
            this.cbFanart.Size = new System.Drawing.Size(89, 17);
            this.cbFanart.TabIndex = 4;
            this.cbFanart.Text = "Enable fanart";
            this.cbFanart.UseVisualStyleBackColor = true;
            this.cbFanart.CheckedChanged += new System.EventHandler(this.cbFanart_CheckedChanged);
            // 
            // lUserId
            // 
            this.lUserId.AutoSize = true;
            this.lUserId.Location = new System.Drawing.Point(6, 86);
            this.lUserId.Name = "lUserId";
            this.lUserId.Size = new System.Drawing.Size(100, 13);
            this.lUserId.TabIndex = 5;
            this.lUserId.Text = "User &ID (medlem nr)";
            // 
            // tbUserId
            // 
            this.tbUserId.Location = new System.Drawing.Point(6, 102);
            this.tbUserId.Name = "tbUserId";
            this.tbUserId.Size = new System.Drawing.Size(207, 20);
            this.tbUserId.TabIndex = 6;
            // 
            // llUserId
            // 
            this.llUserId.AutoSize = true;
            this.llUserId.Location = new System.Drawing.Point(6, 125);
            this.llUserId.Name = "llUserId";
            this.llUserId.Size = new System.Drawing.Size(110, 13);
            this.llUserId.TabIndex = 8;
            this.llUserId.TabStop = true;
            this.llUserId.Text = "Get your User ID here";
            this.llUserId.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llUserId_LinkClicked);
            // 
            // gbAccount
            // 
            this.gbAccount.Controls.Add(this.bUpdateUser);
            this.gbAccount.Controls.Add(this.lApiKey);
            this.gbAccount.Controls.Add(this.llUserId);
            this.gbAccount.Controls.Add(this.tbApiKey);
            this.gbAccount.Controls.Add(this.tbUserId);
            this.gbAccount.Controls.Add(this.llApiKey);
            this.gbAccount.Controls.Add(this.lUserId);
            this.gbAccount.Location = new System.Drawing.Point(12, 12);
            this.gbAccount.Name = "gbAccount";
            this.gbAccount.Size = new System.Drawing.Size(221, 187);
            this.gbAccount.TabIndex = 9;
            this.gbAccount.TabStop = false;
            this.gbAccount.Text = "Add Account";
            // 
            // bUpdateUser
            // 
            this.bUpdateUser.Location = new System.Drawing.Point(6, 154);
            this.bUpdateUser.Name = "bUpdateUser";
            this.bUpdateUser.Size = new System.Drawing.Size(207, 23);
            this.bUpdateUser.TabIndex = 9;
            this.bUpdateUser.Text = "Add Account";
            this.bUpdateUser.UseVisualStyleBackColor = true;
            this.bUpdateUser.Click += new System.EventHandler(this.bUpdateUser_Click);
            // 
            // gbFanart
            // 
            this.gbFanart.Controls.Add(this.llFanartApiKey);
            this.gbFanart.Controls.Add(this.tbFanartApiKey);
            this.gbFanart.Controls.Add(this.lFanartApiKey);
            this.gbFanart.Controls.Add(this.cbPreferFanartPoster);
            this.gbFanart.Controls.Add(this.cbFanart);
            this.gbFanart.Location = new System.Drawing.Point(12, 205);
            this.gbFanart.Name = "gbFanart";
            this.gbFanart.Size = new System.Drawing.Size(221, 137);
            this.gbFanart.TabIndex = 10;
            this.gbFanart.TabStop = false;
            this.gbFanart.Text = "Fanart.tv";
            // 
            // lFanartApiKey
            // 
            this.lFanartApiKey.AutoSize = true;
            this.lFanartApiKey.Location = new System.Drawing.Point(6, 59);
            this.lFanartApiKey.Name = "lFanartApiKey";
            this.lFanartApiKey.Size = new System.Drawing.Size(131, 13);
            this.lFanartApiKey.TabIndex = 6;
            this.lFanartApiKey.Text = "Personal fanart.tv API Key";
            // 
            // cbPreferFanartPoster
            // 
            this.cbPreferFanartPoster.AutoSize = true;
            this.cbPreferFanartPoster.Location = new System.Drawing.Point(9, 39);
            this.cbPreferFanartPoster.Name = "cbPreferFanartPoster";
            this.cbPreferFanartPoster.Size = new System.Drawing.Size(159, 17);
            this.cbPreferFanartPoster.TabIndex = 5;
            this.cbPreferFanartPoster.Text = "Prefer posters from Fanart.tv";
            this.cbPreferFanartPoster.UseVisualStyleBackColor = true;
            this.cbPreferFanartPoster.CheckedChanged += new System.EventHandler(this.cbPreferFanartPoster_CheckedChanged);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 250;
            this.toolTip.AutoPopDelay = 15000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 50;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Help";
            // 
            // gbOnlineVideos
            // 
            this.gbOnlineVideos.Controls.Add(this.lExtraSites);
            this.gbOnlineVideos.Controls.Add(this.tbExtraSites);
            this.gbOnlineVideos.Controls.Add(this.cbMovHunter);
            this.gbOnlineVideos.Controls.Add(this.cbTfPlay);
            this.gbOnlineVideos.Controls.Add(this.cbSweflix);
            this.gbOnlineVideos.Controls.Add(this.cbSwefilmer);
            this.gbOnlineVideos.Controls.Add(this.cbDreamfilm);
            this.gbOnlineVideos.Controls.Add(this.cbViaplay);
            this.gbOnlineVideos.Location = new System.Drawing.Point(239, 205);
            this.gbOnlineVideos.Name = "gbOnlineVideos";
            this.gbOnlineVideos.Size = new System.Drawing.Size(221, 137);
            this.gbOnlineVideos.TabIndex = 11;
            this.gbOnlineVideos.TabStop = false;
            this.gbOnlineVideos.Text = "OnlineVideos search";
            // 
            // lExtraSites
            // 
            this.lExtraSites.AutoSize = true;
            this.lExtraSites.Location = new System.Drawing.Point(6, 85);
            this.lExtraSites.Name = "lExtraSites";
            this.lExtraSites.Size = new System.Drawing.Size(150, 13);
            this.lExtraSites.TabIndex = 7;
            this.lExtraSites.Text = "Other sites (comma separated)";
            // 
            // tbExtraSites
            // 
            this.tbExtraSites.Location = new System.Drawing.Point(6, 111);
            this.tbExtraSites.Name = "tbExtraSites";
            this.tbExtraSites.Size = new System.Drawing.Size(206, 20);
            this.tbExtraSites.TabIndex = 6;
            this.tbExtraSites.TextChanged += new System.EventHandler(this.tbExtraSites_TextChanged);
            // 
            // cbMovHunter
            // 
            this.cbMovHunter.AutoSize = true;
            this.cbMovHunter.Checked = true;
            this.cbMovHunter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMovHunter.Location = new System.Drawing.Point(109, 19);
            this.cbMovHunter.Name = "cbMovHunter";
            this.cbMovHunter.Size = new System.Drawing.Size(79, 17);
            this.cbMovHunter.TabIndex = 5;
            this.cbMovHunter.Text = "MovHunter";
            this.cbMovHunter.UseVisualStyleBackColor = true;
            this.cbMovHunter.CheckedChanged += new System.EventHandler(this.cbMovHunter_CheckedChanged);
            // 
            // cbTfPlay
            // 
            this.cbTfPlay.AutoSize = true;
            this.cbTfPlay.Checked = true;
            this.cbTfPlay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTfPlay.Location = new System.Drawing.Point(109, 36);
            this.cbTfPlay.Name = "cbTfPlay";
            this.cbTfPlay.Size = new System.Drawing.Size(59, 17);
            this.cbTfPlay.TabIndex = 4;
            this.cbTfPlay.Text = "TFPlay";
            this.cbTfPlay.UseVisualStyleBackColor = true;
            this.cbTfPlay.CheckedChanged += new System.EventHandler(this.cbTfPlay_CheckedChanged);
            // 
            // cbSweflix
            // 
            this.cbSweflix.AutoSize = true;
            this.cbSweflix.Checked = true;
            this.cbSweflix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSweflix.Location = new System.Drawing.Point(109, 53);
            this.cbSweflix.Name = "cbSweflix";
            this.cbSweflix.Size = new System.Drawing.Size(59, 17);
            this.cbSweflix.TabIndex = 3;
            this.cbSweflix.Text = "Sweflix";
            this.cbSweflix.UseVisualStyleBackColor = true;
            this.cbSweflix.CheckedChanged += new System.EventHandler(this.cbSweflix_CheckedChanged);
            // 
            // cbSwefilmer
            // 
            this.cbSwefilmer.AutoSize = true;
            this.cbSwefilmer.Checked = true;
            this.cbSwefilmer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSwefilmer.Location = new System.Drawing.Point(9, 53);
            this.cbSwefilmer.Name = "cbSwefilmer";
            this.cbSwefilmer.Size = new System.Drawing.Size(71, 17);
            this.cbSwefilmer.TabIndex = 2;
            this.cbSwefilmer.Text = "Swefilmer";
            this.cbSwefilmer.UseVisualStyleBackColor = true;
            this.cbSwefilmer.CheckedChanged += new System.EventHandler(this.cbSwefilmer_CheckedChanged);
            // 
            // cbDreamfilm
            // 
            this.cbDreamfilm.AutoSize = true;
            this.cbDreamfilm.Checked = true;
            this.cbDreamfilm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDreamfilm.Location = new System.Drawing.Point(9, 36);
            this.cbDreamfilm.Name = "cbDreamfilm";
            this.cbDreamfilm.Size = new System.Drawing.Size(72, 17);
            this.cbDreamfilm.TabIndex = 1;
            this.cbDreamfilm.Text = "Dreamfilm";
            this.cbDreamfilm.UseVisualStyleBackColor = true;
            this.cbDreamfilm.CheckedChanged += new System.EventHandler(this.cbDreamfilm_CheckedChanged);
            // 
            // cbViaplay
            // 
            this.cbViaplay.AutoSize = true;
            this.cbViaplay.Checked = true;
            this.cbViaplay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbViaplay.Location = new System.Drawing.Point(9, 19);
            this.cbViaplay.Name = "cbViaplay";
            this.cbViaplay.Size = new System.Drawing.Size(74, 17);
            this.cbViaplay.TabIndex = 0;
            this.cbViaplay.Text = "Viaplay.se";
            this.cbViaplay.UseVisualStyleBackColor = true;
            this.cbViaplay.CheckedChanged += new System.EventHandler(this.cbViaplay_CheckedChanged);
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(385, 450);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 9;
            this.bOK.Text = "&OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // clbAccounts
            // 
            this.clbAccounts.FormattingEnabled = true;
            this.clbAccounts.Location = new System.Drawing.Point(9, 19);
            this.clbAccounts.Name = "clbAccounts";
            this.clbAccounts.Size = new System.Drawing.Size(206, 124);
            this.clbAccounts.TabIndex = 12;
            // 
            // gbAccounts
            // 
            this.gbAccounts.Controls.Add(this.bRemoveAccounts);
            this.gbAccounts.Controls.Add(this.clbAccounts);
            this.gbAccounts.Location = new System.Drawing.Point(239, 12);
            this.gbAccounts.Name = "gbAccounts";
            this.gbAccounts.Size = new System.Drawing.Size(221, 187);
            this.gbAccounts.TabIndex = 13;
            this.gbAccounts.TabStop = false;
            this.gbAccounts.Text = "Accounts";
            // 
            // bRemoveAccounts
            // 
            this.bRemoveAccounts.Location = new System.Drawing.Point(9, 154);
            this.bRemoveAccounts.Name = "bRemoveAccounts";
            this.bRemoveAccounts.Size = new System.Drawing.Size(206, 23);
            this.bRemoveAccounts.TabIndex = 13;
            this.bRemoveAccounts.Text = "Remove Selected Account(s)";
            this.bRemoveAccounts.UseVisualStyleBackColor = true;
            this.bRemoveAccounts.Click += new System.EventHandler(this.bRemoveAccounts_Click);
            // 
            // gbTvWishList
            // 
            this.gbTvWishList.Controls.Add(this.lTvWishSearchLogic);
            this.gbTvWishList.Controls.Add(this.cbTvWishSearchLogic);
            this.gbTvWishList.Controls.Add(this.cbTvWishListEmail);
            this.gbTvWishList.Location = new System.Drawing.Point(12, 348);
            this.gbTvWishList.Name = "gbTvWishList";
            this.gbTvWishList.Size = new System.Drawing.Size(448, 95);
            this.gbTvWishList.TabIndex = 14;
            this.gbTvWishList.TabStop = false;
            this.gbTvWishList.Text = "TvWishList";
            // 
            // lTvWishSearchLogic
            // 
            this.lTvWishSearchLogic.AutoSize = true;
            this.lTvWishSearchLogic.Location = new System.Drawing.Point(6, 16);
            this.lTvWishSearchLogic.Name = "lTvWishSearchLogic";
            this.lTvWishSearchLogic.Size = new System.Drawing.Size(95, 13);
            this.lTvWishSearchLogic.TabIndex = 2;
            this.lTvWishSearchLogic.Text = "Search TvWish by";
            // 
            // cbTvWishSearchLogic
            // 
            this.cbTvWishSearchLogic.FormattingEnabled = true;
            this.cbTvWishSearchLogic.Location = new System.Drawing.Point(6, 41);
            this.cbTvWishSearchLogic.Name = "cbTvWishSearchLogic";
            this.cbTvWishSearchLogic.Size = new System.Drawing.Size(433, 21);
            this.cbTvWishSearchLogic.TabIndex = 1;
            this.cbTvWishSearchLogic.SelectedIndexChanged += new System.EventHandler(this.cbTvWishSearchLogic_SelectedIndexChanged);
            // 
            // cbTvWishListEmail
            // 
            this.cbTvWishListEmail.AutoSize = true;
            this.cbTvWishListEmail.Location = new System.Drawing.Point(9, 68);
            this.cbTvWishListEmail.Name = "cbTvWishListEmail";
            this.cbTvWishListEmail.Size = new System.Drawing.Size(136, 17);
            this.cbTvWishListEmail.TabIndex = 0;
            this.cbTvWishListEmail.Text = "Send e-mail for TvWish";
            this.cbTvWishListEmail.UseVisualStyleBackColor = true;
            this.cbTvWishListEmail.CheckedChanged += new System.EventHandler(this.cbTvWishListEmail_CheckedChanged);
            // 
            // tbFanartApiKey
            // 
            this.tbFanartApiKey.Location = new System.Drawing.Point(9, 78);
            this.tbFanartApiKey.Name = "tbFanartApiKey";
            this.tbFanartApiKey.Size = new System.Drawing.Size(204, 20);
            this.tbFanartApiKey.TabIndex = 7;
            this.tbFanartApiKey.TextChanged += new System.EventHandler(this.tbFanartApiKey_TextChanged);
            // 
            // llFanartApiKey
            // 
            this.llFanartApiKey.AutoSize = true;
            this.llFanartApiKey.Location = new System.Drawing.Point(9, 111);
            this.llFanartApiKey.Name = "llFanartApiKey";
            this.llFanartApiKey.Size = new System.Drawing.Size(156, 13);
            this.llFanartApiKey.TabIndex = 8;
            this.llFanartApiKey.TabStop = true;
            this.llFanartApiKey.Text = "Get your Personal API Key here";
            this.llFanartApiKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llFanartApiKey_LinkClicked);
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 481);
            this.Controls.Add(this.gbTvWishList);
            this.Controls.Add(this.gbAccounts);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.llRegister);
            this.Controls.Add(this.gbOnlineVideos);
            this.Controls.Add(this.gbFanart);
            this.Controls.Add(this.gbAccount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Configuration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filmtipset";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Configuration_FormClosing);
            this.gbAccount.ResumeLayout(false);
            this.gbAccount.PerformLayout();
            this.gbFanart.ResumeLayout(false);
            this.gbFanart.PerformLayout();
            this.gbOnlineVideos.ResumeLayout(false);
            this.gbOnlineVideos.PerformLayout();
            this.gbAccounts.ResumeLayout(false);
            this.gbTvWishList.ResumeLayout(false);
            this.gbTvWishList.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lApiKey;
        private System.Windows.Forms.TextBox tbApiKey;
        private System.Windows.Forms.LinkLabel llApiKey;
        private System.Windows.Forms.LinkLabel llRegister;
        private System.Windows.Forms.CheckBox cbFanart;
        private System.Windows.Forms.Label lUserId;
        private System.Windows.Forms.TextBox tbUserId;
        private System.Windows.Forms.LinkLabel llUserId;
        private System.Windows.Forms.GroupBox gbAccount;
        private System.Windows.Forms.GroupBox gbFanart;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox gbOnlineVideos;
        private System.Windows.Forms.CheckBox cbViaplay;
        private System.Windows.Forms.CheckBox cbDreamfilm;
        private System.Windows.Forms.CheckBox cbMovHunter;
        private System.Windows.Forms.CheckBox cbTfPlay;
        private System.Windows.Forms.CheckBox cbSweflix;
        private System.Windows.Forms.CheckBox cbSwefilmer;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.CheckBox cbPreferFanartPoster;
        private System.Windows.Forms.Button bUpdateUser;
        private System.Windows.Forms.TextBox tbExtraSites;
        private System.Windows.Forms.Label lExtraSites;
        private System.Windows.Forms.CheckedListBox clbAccounts;
        private System.Windows.Forms.GroupBox gbAccounts;
        private System.Windows.Forms.Button bRemoveAccounts;
        private System.Windows.Forms.GroupBox gbTvWishList;
        private System.Windows.Forms.Label lTvWishSearchLogic;
        private System.Windows.Forms.ComboBox cbTvWishSearchLogic;
        private System.Windows.Forms.CheckBox cbTvWishListEmail;
        private System.Windows.Forms.Label lFanartApiKey;
        private System.Windows.Forms.TextBox tbFanartApiKey;
        private System.Windows.Forms.LinkLabel llFanartApiKey;

    }
}