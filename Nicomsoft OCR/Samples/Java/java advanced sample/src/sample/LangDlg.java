/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package sample;

import javax.swing.*;
import java.awt.* ;
import NSOCR.*;

/**
 *
 * @author Now
 */
public class LangDlg extends javax.swing.JDialog {

    private String[] Language = {"Bulgarian", "Catalan", "Croatian", "Czech",
    "Danish", "Dutch", "English", "Estonian", "Finnish", "French", "German", 
    "Hungarian", "Indonesian", "Italian", "Latvian", "Lithuanian", "Norwegian",
    "Polish", "Portuguese", "Romanian", "Russian", "Slovak", "Slovenian", 
    "Spanish", "Swedish", "Turkish"};
    
    private String[] LanguageAsian = {"Arabic", "Chinese_Simplified", "Chinese_Traditional", "Japanese",
    "Korean"};    
    
    private JCheckBox[] cbLanguage;
    private JCheckBox[] cbLanguageAsian;
    private HCFG hcfg;
    
    /**
     * Creates new form LangDlg
     */
    public LangDlg(java.awt.Frame parent, boolean modal) {
        super(parent, modal);
        initComponents();
        
        setTitle("Select Language");                        
    }
    
    public void init(HCFG hcfg)
    {
        this.hcfg = hcfg;
        
        GridBagConstraints c =  new GridBagConstraints();
               
        c.anchor = GridBagConstraints.NORTH; 
        c.fill   = GridBagConstraints.HORIZONTAL;  
        c.gridheight = 1;
        c.gridwidth  = GridBagConstraints.REMAINDER; 
        c.gridx = GridBagConstraints.RELATIVE; 
        c.gridy = GridBagConstraints.RELATIVE; 
        c.insets = new Insets(0, 10, 0, 0);
        c.ipadx = 0;
        c.ipady = 0;
        c.weightx = 1.0;
        c.weighty = 0.0;        
        
        cbLanguage = new JCheckBox[Language.length];
        cbLanguageAsian = new JCheckBox[LanguageAsian.length];
        JCheckBox cb;
        
        StringBuffer val = new StringBuffer();
        
        for (int i=0; i < Language.length; i++)
        {            
            cbLanguage[i] = new JCheckBox(Language[i]);
            cb = cbLanguage[i];
            
            int res = NSOCR.Engine.Cfg_GetOption(hcfg, NSOCR.Constant.BT_DEFAULT, "Languages/"+Language[i], val);
            
	    if (res < NSOCR.Error.ERROR_FIRST)
            {
                cb.setSelected(val.toString().equals("1") ? true : false);
            }
            else
                cb.setSelected(false);
            
            jPanel1.add(cb, c);   
        }        
        for (int i=0; i < LanguageAsian.length; i++)
        {            
            cbLanguageAsian[i] = new JCheckBox(LanguageAsian[i]);
            cb = cbLanguageAsian[i];
            
            int res = NSOCR.Engine.Cfg_GetOption(hcfg, NSOCR.Constant.BT_DEFAULT, "Languages/"+LanguageAsian[i], val);
            
	    if (res < NSOCR.Error.ERROR_FIRST)
            {
                cb.setSelected(val.toString().equals("1") ? true : false);
            }
            else
                cb.setSelected(false);
            
            jPanel2.add(cb, c);   
        }         
    }
    
    public boolean ApplyLanguages()
    {
        boolean ch1 = false;     
        for (int i = 0; i < cbLanguage.length; i++)
        {
            if (cbLanguage[i].isSelected())
            {
                ch1 = true;
                break;
            }
        }
        int ch2 = 0;     
        for (int i = 0; i < cbLanguageAsian.length; i++)
        {
            if (cbLanguageAsian[i].isSelected())
                ch2++;
        }        
        if (!ch1 && (ch2 == 0))
        {
            JOptionPane.showMessageDialog(this, "Please select at least one language for recognition.");
            return false;
        }
        if (ch1 && (ch2 > 0))
        {
            JOptionPane.showMessageDialog(this, "Using both main and asian languages in same zone is not supported.");
            return false;
        }
        if (ch2 > 1)
        {
            JOptionPane.showMessageDialog(this, "Using two or more asian languages in same zone is not supported currently.");
            return false;
        }
         
        for (int i = 0; i < cbLanguage.length; i++)
	{
            NSOCR.Engine.Cfg_SetOption(hcfg, NSOCR.Constant.BT_DEFAULT, "Languages/"+Language[i], cbLanguage[i].isSelected() ? "1" : "0");
	}
        for (int i = 0; i < cbLanguageAsian.length; i++)
	{
            NSOCR.Engine.Cfg_SetOption(hcfg, NSOCR.Constant.BT_DEFAULT, "Languages/"+LanguageAsian[i], cbLanguageAsian[i].isSelected() ? "1" : "0");
	}
        return true;
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jScrollPane1 = new javax.swing.JScrollPane();
        jPanel1 = new javax.swing.JPanel();
        btnOk = new javax.swing.JButton();
        btnCancel = new javax.swing.JButton();
        jScrollPane2 = new javax.swing.JScrollPane();
        jPanel2 = new javax.swing.JPanel();
        jLabel1 = new javax.swing.JLabel();
        jLabel2 = new javax.swing.JLabel();
        jLabel3 = new javax.swing.JLabel();

        setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);
        setResizable(false);

        jScrollPane1.setHorizontalScrollBarPolicy(javax.swing.ScrollPaneConstants.HORIZONTAL_SCROLLBAR_NEVER);

        java.awt.GridBagLayout jPanel1Layout = new java.awt.GridBagLayout();
        jPanel1Layout.rowHeights = new int[] {20};
        jPanel1.setLayout(jPanel1Layout);
        jScrollPane1.setViewportView(jPanel1);

        btnOk.setText("OK");
        btnOk.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnOkActionPerformed(evt);
            }
        });

        btnCancel.setText("Cancel");
        btnCancel.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnCancelActionPerformed(evt);
            }
        });

        jScrollPane2.setHorizontalScrollBarPolicy(javax.swing.ScrollPaneConstants.HORIZONTAL_SCROLLBAR_NEVER);

        jPanel2.setLayout(new java.awt.GridBagLayout());
        jScrollPane2.setViewportView(jPanel2);

        jLabel1.setText("Asian module languages:");

        jLabel2.setText("Main module languages:");

        jLabel3.setText("<html>NOTE: Using both main and asian languages in one zone is not supported.<br>\nBut you can use both main and asian languages on same image in different zones.<br>\nCurrently Asian languages are not supported for Linux platform.</html>");

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jLabel2)
                            .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 175, javax.swing.GroupLayout.PREFERRED_SIZE))
                        .addGap(37, 37, 37)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jLabel1)
                            .addComponent(jScrollPane2, javax.swing.GroupLayout.PREFERRED_SIZE, 176, javax.swing.GroupLayout.PREFERRED_SIZE))
                        .addContainerGap(32, Short.MAX_VALUE))
                    .addGroup(layout.createSequentialGroup()
                        .addGap(135, 135, 135)
                        .addComponent(btnOk, javax.swing.GroupLayout.PREFERRED_SIZE, 59, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(btnCancel)
                        .addGap(0, 0, Short.MAX_VALUE))
                    .addComponent(jLabel3)))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jLabel2)
                    .addComponent(jLabel1))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 641, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jScrollPane2, javax.swing.GroupLayout.PREFERRED_SIZE, 312, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(jLabel3, javax.swing.GroupLayout.DEFAULT_SIZE, 58, Short.MAX_VALUE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(btnCancel)
                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
                        .addComponent(btnOk)
                        .addContainerGap())))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void btnCancelActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnCancelActionPerformed
        this.dispose();
    }//GEN-LAST:event_btnCancelActionPerformed

    private void btnOkActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnOkActionPerformed
        
        if (ApplyLanguages()) this.dispose();
    }//GEN-LAST:event_btnOkActionPerformed

   
    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton btnCancel;
    private javax.swing.JButton btnOk;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JPanel jPanel2;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JScrollPane jScrollPane2;
    // End of variables declaration//GEN-END:variables
}
