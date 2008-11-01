using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Jez {
	public class JezRichTextBox : System.Windows.Forms.RichTextBox {
		Color disabledBackColor;
		Color originalBackColor;
		
		public JezRichTextBox() {
			this.disabledBackColor = Color.LightBlue;
		}
		
		protected override void OnEnabledChanged(EventArgs e) {
			// TODO: Figure out a way to get the background color we set to get painted
			// even when the control is disabled.  At the moment it always defaults
			// to grey when disabled.  :-(
			if (!this.Enabled) {
				this.originalBackColor = this.BackColor;
				this.BackColor = disabledBackColor;
			}
			else {
				this.BackColor = originalBackColor;
			}
			
			base.OnEnabledChanged(e);
		}
		
		[Browsable(true)]
		public Color DisabledBackColor {
			get {
				return this.disabledBackColor;
			}
			set {
				this.disabledBackColor = value;
			}
		}
	}
}
