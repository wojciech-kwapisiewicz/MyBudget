using MyBudget.Classification;
using MyBudget.UI.Core.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core.Services
{
    public class ResolveClassificationConflicts : IResolveClassificationConflicts
    {
        IMessageBoxService _messageBoxService;

        public ResolveClassificationConflicts(IMessageBoxService messageBoxService)
        {
            _messageBoxService = messageBoxService;
        }

        public void ResolveConflicts(IEnumerable<ClassificationResult> classificationResult)
        {
            var withConflicts = classificationResult.Where(a => a.Matches.Count() > 1).ToArray();

            string txt = string.Empty;
            if (withConflicts.Length > 0)
            {
                foreach (var abc in withConflicts)
                {
                    txt += abc.BankOperation.Title;
                    foreach (var bcd in abc.Matches)
                    {
                        txt += " ";
                        txt += bcd.MatchedDefinition.Description;
                    }
                    txt += Environment.NewLine;
                }

                string content = string.Format(
                    Resources.Translations.ConflictsTextTemplate,
                    withConflicts.Count(),
                    Environment.NewLine,
                    txt);

                _messageBoxService.ShowMessageBox("Info", content);
            }
        }
    }
}
