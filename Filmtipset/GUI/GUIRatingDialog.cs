using System;
using System.Drawing;
using System.IO;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;
using Alignment = MediaPortal.GUI.Library.GUIControl.Alignment;
using VAlignment = MediaPortal.GUI.Library.GUIControl.VAlignment;
using MediaPortal.Dialogs;


namespace Filmtipset.GUI
{
    public class GUIRatingDialog : GUIDialogWindow
    {
        [SkinControl(1)]
        protected GUILabelControl lblHeading = null;

        [SkinControl(2)]
        protected GUIFadeLabel lblLine1 = null;

        [SkinControl(3)]
        protected GUIFadeLabel lblLine2 = null;

        [SkinControl(4)]
        protected GUIFadeLabel lblLine3 = null;

        [SkinControl(10)]
        protected GUILabelControl lblRating = null;

        [SkinControl(100)]
        protected GUIButtonControl btnGrade0 = null;
        [SkinControl(101)]
        protected GUIButtonControl btnGrade1 = null;
        [SkinControl(102)]
        protected GUIButtonControl btnGrade2 = null;
        [SkinControl(103)]
        protected GUIButtonControl btnGrade3 = null;
        [SkinControl(104)]
        protected GUIButtonControl btnGrade4 = null;
        [SkinControl(105)]
        protected GUIButtonControl btnGrade5 = null;

        private bool m_bNeedRefresh = false;

        #region Public Properties

        public int Rated { get; set; }
        public bool IsSubmitted { get; set; }

        #endregion

        public GUIRatingDialog()
        {
            GetID = (int)FilmtipsetGUIWindows.RatingDialog;
        }

        public override bool Init()
        {
            return Load(GUIGraphicsContext.GetThemedSkinFile(@"\Filmtipset.RateDialog.xml"));
        }

        #region Base Dialog Members

        public override void DoModal(int dwParentId)
        {
            base.DoModal(dwParentId);
        }

        #endregion

        public override void OnAction(Action action)
        {
            switch (action.wID)
            {
                case Action.ActionType.REMOTE_1:
                        Rated = 1;
                    UpdateRating();
                    break;

                case Action.ActionType.REMOTE_2:
                        Rated = 2;
                    UpdateRating();
                    break;

                case Action.ActionType.REMOTE_3:
                        Rated = 3;
                    UpdateRating();
                    break;

                case Action.ActionType.REMOTE_4:
                        Rated = 4;
                    UpdateRating();
                    break;

                case Action.ActionType.REMOTE_5:
                        Rated = 5;
                    UpdateRating();
                    break;

                case Action.ActionType.REMOTE_0:
                        Rated = 0;
                    UpdateRating();
                    break;

                case Action.ActionType.ACTION_KEY_PRESSED:
                    // some types of remotes send ACTION_KEY_PRESSED instead of REMOTE_0 - REMOTE_9 commands
                    if (action.m_key != null)
                    {
                        int key = action.m_key.KeyChar;
                        if (key >= '0' && key <= '5')
                        {
                            Rated = 0;
                            UpdateRating();
                        }
                    }
                    break;

                case Action.ActionType.ACTION_SELECT_ITEM:
                    IsSubmitted = true;
                    PageDestroy();
                    return;

                case Action.ActionType.ACTION_PREVIOUS_MENU:
                case Action.ActionType.ACTION_CLOSE_DIALOG:
                case Action.ActionType.ACTION_CONTEXT_MENU:
                    IsSubmitted = false;
                    PageDestroy();
                    return;
            }

            base.OnAction(action);
        }

        public override bool OnMessage(GUIMessage message)
        {
            //needRefresh = true;
            switch (message.Message)
            {
                case GUIMessage.MessageType.GUI_MSG_WINDOW_INIT:
                    // store old rating so default control in skin does override
                    int rating = Rated;

                    base.OnMessage(message);

                    // readjust rating and default control focus
                    if (rating > 0 && rating < 6)
                    {
                        int defaultControlId = 100 + rating;
                        GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_SETFOCUS, GetID, 0, defaultControlId, 0, 0, null);
                        OnMessage(msg);
                    }

                    IsSubmitted = false;
                    UpdateRating();
                    return true;

                case GUIMessage.MessageType.GUI_MSG_SETFOCUS:
                    if (btnGrade0 != null && message.TargetControlId == btnGrade0.GetID)
                    {
                        Rated = 0;
                        UpdateRating();
                        break;
                    }

                    if (message.TargetControlId < 101 || message.TargetControlId > 105)
                        break;
                    Rated = message.TargetControlId - 100;
                    UpdateRating();
                    break;
            

            }

            return base.OnMessage(message);
        }

        public void SetHeading(string strLine)
        {
            //LoadSkin();
            AllocResources();
            InitControls();

            lblHeading.Label = strLine;
            lblLine1.Label = "";
            lblLine2.Label = "";
            lblLine3.Label = "";
        }

        public void SetRating(string strLine)
        {
            lblRating.Label = strLine;
        }

        public void SetMovieName(string strLine)
        {
            lblLine1.Label = strLine;
        }

        public void SetHeading(int iString)
        {
            SetHeading(Translation.GetByName("Filmtipset"));
        }

        public override bool NeedRefresh()
        {
            if (m_bNeedRefresh)
            {
                m_bNeedRefresh = false;
                return true;
            }
            return false;
        }

        private void UpdateRating()
        {
            btnGrade0.Selected = (Rated == 0);
            btnGrade0.Focus = (Rated == 0);
            btnGrade1.Selected = (Rated == 1);
            btnGrade1.Focus = (Rated == 1);
            btnGrade2.Selected = (Rated == 2);
            btnGrade2.Focus = (Rated == 2);
            btnGrade3.Selected = (Rated == 3);
            btnGrade3.Focus = (Rated == 3);
            btnGrade4.Selected = (Rated == 4);
            btnGrade4.Focus = (Rated == 4);
            btnGrade5.Selected = (Rated == 5);
            btnGrade5.Focus = (Rated == 5);
            lblRating.Label = "Sätt betyg " + Rated;
        }

    }
}