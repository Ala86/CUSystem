using System;

namespace TclassLibrary
{
    internal class REPTrialBalance
    {
        private string cLocalCaption;
        private string cs;
        private int ncompid;

        public REPTrialBalance(string cs, int ncompid, string cLocalCaption)
        {
            this.cs = cs;
            this.ncompid = ncompid;
            this.cLocalCaption = cLocalCaption;
        }

        internal void ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}