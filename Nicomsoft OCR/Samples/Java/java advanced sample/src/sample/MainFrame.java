/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package sample;

import  NSOCR.*; 
import  javax.swing.JOptionPane;
import  javax.swing.*;
import  java.awt.*;
import  java.awt.event.*;
import  java.awt.image.*;
import  java.io.*;
import  javax.imageio.*;
/**
 *
 * @author Now
 */
public class MainFrame extends javax.swing.JFrame {
    
//-----------------------------------------------------------------------------    
    public void finalize()
    {
        if (ImgObj.GetValue() != 0) NSOCR.Engine.Img_Destroy(ImgObj);
	if (OcrObj.GetValue() != 0) NSOCR.Engine.Ocr_Destroy(OcrObj);
	if (CfgObj.GetValue() != 0) NSOCR.Engine.Cfg_Destroy(CfgObj);
	if (ScanObj.GetValue() != 0) NSOCR.Engine.Scan_Destroy(ScanObj);        
        if (SvrObj.GetValue() != 0) NSOCR.Engine.Svr_Destroy(SvrObj);
    }
    
//-----------------------------------------------------------------------------        
    /**
     * Creates new form MainFrame
     */
    public MainFrame() 
    {
        initComponents();
        
        DocImg = new nsDrawPanel();
        
        GridBagConstraints c = new GridBagConstraints();
        c.gridx=0;
        c.gridy=0;
        c.weightx = 1.0;
        c.weighty = 1.0;
        c.gridx = 1;
        c.gridy = 0;
        c.gridheight = 4;
        c.fill = GridBagConstraints.BOTH;
        
        jPanel2.add(DocImg, c);
        DocImg.setSize(100, 100);
        
        int w = DocImg.getWidth();
        int h = DocImg.getHeight();
        
        DocImg.setEnabled(true);
        DocImg.setVisible(true);
        
                
        jLabel4.setVisible(false);
        
        CfgObj  = new HCFG();
        ImgObj  = new HIMG();
        OcrObj  = new HOCR();
        ScanObj = new HSCAN();
        SvrObj  = new HSVR();
        Frame   = new Rect();
        
        Dwn = false;
        IsProcessPagesMode = false;
        
        bmp = new BufferedImage(1, 1, BufferedImage.TYPE_INT_ARGB);
                
        boolean ok = NSOCR.Engine.IsDllLoaded();
        
        if (!ok)
        {
            JOptionPane.showMessageDialog(this, "NSOCR library not loaded!");
            System.exit(1);
        }
   
        StringBuffer ver = new StringBuffer("");
        NSOCR.Engine.Engine_GetVersion(ver);            
        String title = "Nicomsoft OCR JAVA Advanced Sample [NSOCR version: " + ver + " ]";
        this.setTitle(title);	
    
        NSOCR.Engine.Engine_SetLicenseKey(""); //required for licensed version only
        
        //Initialize OCR, create CFG, OCR and IMG objects, load configuration
        NSOCR.Engine.Engine_InitializeAdvanced(CfgObj, OcrObj, ImgObj);
        NSOCR.Engine.Scan_Create(CfgObj, ScanObj); //create SCAN object
          
        //copy some settings to GUI
        StringBuffer val = new StringBuffer(256);
        
	if (NSOCR.Engine.Cfg_GetOption(CfgObj, NSOCR.Constant.BT_DEFAULT, "ImgAlizer/AutoScale", val) < NSOCR.Error.ERROR_FIRST)
		cbScale.setEnabled(val.equals(val));
        
       //by default this option is disabled because it takes about 10% of total recognition time
       //enable it to demonstrate this feature
       //NSOCR.Engine.Cfg_SetOption(CfgObj, NSOCR.Constant.BT_DEFAULT, "Zoning/FindBarcodes", "1");
       
       //also enable auto-detection of image inversion
       //NSOCR.Engine.Cfg_SetOption(CfgObj, NSOCR.Constant.BT_DEFAULT, "ImgAlizer/Inversion", "2");        
        
//////////////////
	
	btnRecognize.setEnabled(false);
   	btnSave.setEnabled(false);
	cbScale.setSelectedIndex(0);
    }
//-----------------------------------------------------------------------------      
   private boolean IsDelimiter(char ch)
   {
	return (ch == '\\') || (ch == ':');
   }
//-----------------------------------------------------------------------------         
   private boolean IsImgLoaded()
   {
        NSInt width   = new NSInt(0);
        NSInt height  = new NSInt(0);
   
	if (NSOCR.Engine.Img_GetSize(ImgObj, width, height) > NSOCR.Error.ERROR_FIRST) return false;      
	return (width.Value > 0) && (height.Value > 0); 
   }
//-----------------------------------------------------------------------------        
    private void AdjustDocScale()
    {
	if (!IsImgLoaded()) return;
        
        NSInt width   = new NSInt(jPanel2.getWidth());
        NSInt height  = new NSInt(jPanel2.getHeight());
        
        if (width.Value <= 0 || height.Value <= 0) return;
        
        DocImg.setSize(width.GetValue(), height.GetValue());               
        
        int bmpdata[] = new int[width.Value * height.Value];
        
	if (cbDispBin.isSelected()) 
        {
            NSOCR.Engine.Img_GetBmpData(ImgObj, bmpdata, width, height,NSOCR.Constant.DRAW_BINARY); 
        }
	else NSOCR.Engine.Img_GetBmpData(ImgObj, bmpdata, width, height, NSOCR.Constant.DRAW_NORMAL);
        
        if (width.Value == 0 || height.Value == 0) return;              
        bmp = new BufferedImage(width.Value, height.Value, BufferedImage.TYPE_INT_ARGB);
              
        bmp.setRGB(0, 0, width.GetValue(), height.GetValue(), bmpdata, 0, width.GetValue());
                
        ShowImage();    
    }
    
//-----------------------------------------------------------------------------    
    private float GetCurDocScale()
    {
	if (!IsImgLoaded()) return 1.0f;
       
	int w = DocImg.getWidth();
	int h = DocImg.getHeight();

        NSInt width   = new NSInt(0);
        NSInt height  = new NSInt(0);
        
	NSOCR.Engine.Img_GetSize(ImgObj, width, height);
        
	float kX = (float)w / width.Value;
	float kY = (float)h / height.Value;
        
	float k;
        
	if (kX > kY) k = kY;
	else k = kX;

	return k;        
    }
    
//-----------------------------------------------------------------------------    
    public void ShowImage()
    {
	if (!IsImgLoaded()) return;

	BufferedImage bmp2;
        
        int w = bmp.getWidth();
        int h = bmp.getHeight();
        
        bmp2 = new BufferedImage (w, h, BufferedImage.TYPE_INT_ARGB);
        bmp2.getGraphics().drawImage(bmp, 0, 0, this);

	float k = GetCurDocScale();
        
	//RECT r;
        int i, left, top, right, bottom;
        HBLK BlkObj = new HBLK();
        java.awt.Color col = new java.awt.Color(0,0,0);
        
        NSInt Xpos   = new NSInt(0);
        NSInt Ypos   = new NSInt(0); 
        NSInt Width  = new NSInt(0); 
        NSInt Height = new NSInt(0);
               
        int cnt = NSOCR.Engine.Img_GetBlockCnt(ImgObj);
        
        for (i = 0; i < cnt; i++)
        {
            NSOCR.Engine.Img_GetBlock(ImgObj, i, BlkObj);
            NSOCR.Engine.Blk_GetRect(BlkObj, Xpos, Ypos, Width, Height);

            left   = (int)(k * (float)Xpos.Value);
            top    = (int)(k * (float)Ypos.Value);
            right  = (int)(k * ((float)Xpos.Value + (float)Width.Value - 1)) + 1;
            bottom = (int)(k * ((float)Ypos.Value + (float)Height.Value - 1)) + 1;

            switch (NSOCR.Engine.Blk_GetType(BlkObj))
            {
                case NSOCR.Constant.BT_OCRTEXT: col = NSOCR.Color.NclGreen; break;
		case NSOCR.Constant.BT_OCRDIGIT:col = NSOCR.Color.NclLime;  break;
                case NSOCR.Constant.BT_ICRDIGIT:col = NSOCR.Color.NclBlue;  break;
                case NSOCR.Constant.BT_PICTURE: col = NSOCR.Color.NclAqua;  break;
                case NSOCR.Constant.BT_CLEAR:   col = NSOCR.Color.NclGray;  break;
		case NSOCR.Constant.BT_ZONING:  col = NSOCR.Color.NclBlack; break;
		case NSOCR.Constant.BT_BARCODE: col = NSOCR.Color.NclNavy;  break;
		case NSOCR.Constant.BT_TABLE:   col = NSOCR.Color.NclOlive; break;
                case NSOCR.Constant.BT_MRZ:     col = NSOCR.Color.NclBlack; break;
            }
    
            Graphics2D gr = (Graphics2D)bmp2.getGraphics();
            
            gr.setColor(col);
            gr.setStroke(new BasicStroke(2));
            gr.drawRect(left, top, right-left, bottom-top);

            String BlockNum = String.valueOf(i);
	    gr.drawString(BlockNum, left + 1, top + 10);
       }

	//user is creating new block, draw its frame
	if (Dwn)
	{
		left = (int)(k * Frame.left);
		top = (int)(k * Frame.top);
		right = (int)(k * Frame.right);
		bottom = (int)(k * Frame.bottom);

		w = bmp2.getWidth();
		h = bmp2.getHeight();

		if (right > w - 1) right = w - 1;
		if (bottom > h - 1) bottom = h - 1;

                Graphics gr = bmp2.getGraphics();
                
                gr.setColor(NSOCR.Color.NclRed);
                gr.drawRect(left, top, right-left, bottom-top);                
	}
         
        DocImg.setImage(bmp2); 
    }
    
//-----------------------------------------------------------------------------        
    private void ShowText()
    {
	int flags = jCheckBox1.isSelected() ? NSOCR.Constant.FMT_EXACTCOPY : NSOCR.Constant.FMT_EDITCOPY;
        
        StringBuffer text = new StringBuffer();
	NSOCR.Engine.Img_GetImgText(ImgObj, text, flags);

	tpText.setText(text.toString());

    }
//-----------------------------------------------------------------------------        
    private void ProcessEntireDocument()
    {
        IsProcessPagesMode = true;
        SaveDocument();
        
        if (SvrObj.Value == 0) return; //saving cancelled

        int OcrCnt, res;
        boolean InSameThread;
        
        //recognize up to 4 images at once.
        //Note for large images ERROR_NOMEMORY can be raised
        //OcrCnt = 4;

        //Use number of logical CPU cores on the system for the best performance
        OcrCnt = 0;

        InSameThread = false; //perform OCR in non-blocking mode
        //InSameThread = true; //uncomment to perform OCR from this thread (GUI will be freezed)

        int flags = jCheckBox1.isSelected() ? NSOCR.Constant.FMT_EXACTCOPY : NSOCR.Constant.FMT_EDITCOPY;
         
        flags = flags << 8; //we need to shift FMT_XXXXX flags for this function

        //process all pages of input image and add results to SAVER object
        //this function will create (and then release) additional temporary OCR objects if OcrCnt > 1
        if (InSameThread)
	{
	  res = NSOCR.Engine.Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt, NSOCR.Constant.OCRFLAG_NONE | flags);
	}
        else
	{
	  //do it in non-blocking mode and then wait for result
	  res = NSOCR.Engine.Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt, NSOCR.Constant.OCRFLAG_THREAD | flags);
	  if (res > NSOCR.Error.ERROR_FIRST)
	  {
                JOptionPane.showMessageDialog(this, "Ocr_ProcessPages(1)" + Integer.toHexString(res));
		return;
	  }
          
	  dlgWait dlg = new dlgWait(this, true);
	  dlg.init(ImgObj, 1);
          dlg.setLocationRelativeTo(this);
	  dlg.setVisible(true);
	  res = dlg.res;
          
          JOptionPane.showMessageDialog(this, "Done!");
	}
        
        //restore option
        NSOCR.Engine.Cfg_SetOption(CfgObj, NSOCR.Constant.BT_DEFAULT, "Main/NumKernels", "0");

        if (res > NSOCR.Error.ERROR_FIRST)
        {
            if (res == NSOCR.Error.ERROR_OPERATIONCANCELLED)
            {
                JOptionPane.showMessageDialog(this, "Operation was cancelled.");
            }
            else
            {
		JOptionPane.showMessageDialog(this, "Ocr_ProcessPages" + Integer.toHexString(res));
            }
            
            NSOCR.Engine.Svr_Destroy(SvrObj);
            return;
        }

        //save output document
	res = NSOCR.Engine.Svr_SaveToFile(SvrObj, SavedFileName);
	NSOCR.Engine.Svr_Destroy(SvrObj);

	if (res > NSOCR.Error.ERROR_FIRST)
	{
            JOptionPane.showMessageDialog(this, "Svr_SaveToFile" + Integer.toHexString(res));
	}

	//open the file
//	ShellExecute(0, L"open", fn, L"", NULL, SW_SHOWNORMAL ); //*/
}
//-----------------------------------------------------------------------------    
private void SaveDocument()
{
SvrObj.SetValue(0);
	boolean ppMode = IsProcessPagesMode;
        
	IsProcessPagesMode = false;

        JFileChooser chooser = new JFileChooser();
        javax.swing.filechooser.FileNameExtensionFilter filter;       
        
        filter = new javax.swing.filechooser.FileNameExtensionFilter(
            "ASCII Text document (*.txt)", "txt");
        chooser.setFileFilter(filter);
        
        filter = new javax.swing.filechooser.FileNameExtensionFilter(
            "Unicode Text document (*.txt)", "txt");       
        chooser.setFileFilter(filter);
        
        filter = new javax.swing.filechooser.FileNameExtensionFilter(
                "RTF document (*.rtf)", "rtf");
            chooser.setFileFilter(filter);

        filter = new javax.swing.filechooser.FileNameExtensionFilter(
            "XML document (*.xml)", "xml");
        chooser.setFileFilter(filter);  
        
        filter = new javax.swing.filechooser.FileNameExtensionFilter(
                "PDF/A document (*.pdf)", "pdf");
            chooser.setFileFilter(filter);     
            
        filter = new javax.swing.filechooser.FileNameExtensionFilter(
                "PDF document (*.pdf)", "pdf");
            chooser.setFileFilter(filter);             
            
        chooser.setAcceptAllFileFilterUsed(false);
        int returnVal = chooser.showSaveDialog(this);        

        if (returnVal == JFileChooser.APPROVE_OPTION)
        {
            java.io.File fl = chooser.getSelectedFile();
            filter = (javax.swing.filechooser.FileNameExtensionFilter) chooser.getFileFilter();

            int format = NSOCR.Constant.SVR_FORMAT_PDF;             
            if (filter.getDescription().equalsIgnoreCase("Unicode Text document (*.txt)")) format = NSOCR.Constant.SVR_FORMAT_TXT_UNICODE;   
            if (filter.getDescription().equalsIgnoreCase("ASCII Text document (*.txt)")) format = NSOCR.Constant.SVR_FORMAT_TXT_ASCII;   
            if (filter.getDescription().equalsIgnoreCase("RTF document (*.rtf)")) format = NSOCR.Constant.SVR_FORMAT_RTF;               
            if (filter.getDescription().equalsIgnoreCase("XML document (*.xml)")) format = NSOCR.Constant.SVR_FORMAT_XML;  
            if (filter.getDescription().equalsIgnoreCase("PDF/A document (*.pdf)")) format = NSOCR.Constant.SVR_FORMAT_PDFA; 
            if (filter.getDescription().equalsIgnoreCase("PDF document (*.pdf)")) format = NSOCR.Constant.SVR_FORMAT_PDF;  
            
            SavedFileName  = fl.getAbsolutePath();
            
            //image over text option for PDF
            if ((format == NSOCR.Constant.SVR_FORMAT_PDF) || (format == NSOCR.Constant.SVR_FORMAT_PDFA))
            {
                int sel = JOptionPane.showConfirmDialog(this,
                      "Place page image over recognized text?", "", JOptionPane.YES_NO_OPTION);
            
                if (sel == JOptionPane.YES_OPTION)
                {
                    NSOCR.Engine.Cfg_SetOption(CfgObj, NSOCR.Constant.BT_DEFAULT, "Saver/PDF/ImageLayer", "1");		
                }                
                else
                {
                    NSOCR.Engine.Cfg_SetOption(CfgObj, NSOCR.Constant.BT_DEFAULT, "Saver/PDF/ImageLayer", "0");		
                }            
            }

            int res = NSOCR.Engine.Svr_Create(CfgObj, format, SvrObj);
            if (res > NSOCR.Error.ERROR_FIRST)
            {
                JOptionPane.showMessageDialog(this, "Svr_Create" + Integer.toHexString(res));
		return;
            }

            int flags = jCheckBox1.isSelected() ? NSOCR.Constant.FMT_EXACTCOPY : NSOCR.Constant.FMT_EDITCOPY;

            NSOCR.Engine.Svr_NewDocument(SvrObj);

            if (ppMode) //caller is ProcessEntireDocument
		return;

            res = NSOCR.Engine.Svr_AddPage(SvrObj, ImgObj, flags);
            if (res > NSOCR.Error.ERROR_FIRST)
            {
                JOptionPane.showMessageDialog(this, "Svr_AddPage " + Integer.toHexString(res));
		NSOCR.Engine.Svr_Destroy(SvrObj);
		return;
            }

            if ((format == NSOCR.Constant.SVR_FORMAT_PDF) || (format == NSOCR.Constant.SVR_FORMAT_XML)) //demonstrate how to write PDF info
		NSOCR.Engine.Svr_SetDocumentInfo(SvrObj, NSOCR.Constant.INFO_PDF_TITLE, "Sample Title");

            res = NSOCR.Engine.Svr_SaveToFile(SvrObj, SavedFileName);
            NSOCR.Engine.Svr_Destroy(SvrObj);

            if (res > NSOCR.Error.ERROR_FIRST)
            {
		JOptionPane.showMessageDialog(this, "Svr_SaveToFile " + Integer.toHexString(res));
            }
        }
}
//-----------------------------------------------------------------------------    
    private void DoImageLoaded()
    {
	//clear bitmaps for old image

	int res;

	//check if image has multiple page and ask user if he wants process and save all pages automatically
	res = NSOCR.Engine.Img_GetPageCount(ImgObj);
        
	if (res > NSOCR.Error.ERROR_FIRST)
	{
            JOptionPane.showMessageDialog(this, "Img_GetPageCount" + Integer.toHexString(res));
            return;
	}
        
	if (res > 1)
	{
            int sel = JOptionPane.showConfirmDialog(this,
                      "Image contains multiple pages " + res + ". Do you want to process and save all pages automatically?",
                      "Question", JOptionPane.YES_NO_OPTION);
            
                if (sel == JOptionPane.YES_OPTION)
                {
                    ProcessEntireDocument();
                    btnRecognize.setEnabled(false);
                    return;
		}
	}

	//now apply image scaling, binarize image, deskew etc,
	//everything except OCR itself
	res = NSOCR.Engine.Img_OCR(ImgObj, NSOCR.Constant.OCRSTEP_FIRST, NSOCR.Constant.OCRSTEP_ZONING - 1, NSOCR.Constant.OCRFLAG_NONE);
        
	if (res > NSOCR.Error.ERROR_FIRST) 
            JOptionPane.showMessageDialog(this, "Img_OCR" + Integer.toHexString(res));

	tfPage.setText("1");
        
	int cnt = NSOCR.Engine.Img_GetPageCount(ImgObj);
	String str = "of " + cnt;
        jLabel2.setText(str);
        
	btnScan.setEnabled(true);

	AdjustDocScale();

	btnRecognize.setEnabled(true);
	tpText.setText("");

	btnLoadZones.setEnabled(true);
	btnSaveZones.setEnabled(true);
	btnClearZones.setEnabled(true);
	btnDetectZones.setEnabled(true);
    }
    
    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jPanel3 = new javax.swing.JPanel();
        btnScan = new javax.swing.JButton();
        btnOpenFile = new javax.swing.JButton();
        btnRecognize = new javax.swing.JButton();
        btnSave = new javax.swing.JButton();
        jLabel1 = new javax.swing.JLabel();
        tfPage = new javax.swing.JTextField();
        jLabel2 = new javax.swing.JLabel();
        btnSetPage = new javax.swing.JButton();
        jCheckBox1 = new javax.swing.JCheckBox();
        jLabel3 = new javax.swing.JLabel();
        cbScale = new javax.swing.JComboBox();
        jLabel4 = new javax.swing.JLabel();
        btnSetLang = new javax.swing.JButton();
        jButton1 = new javax.swing.JButton();
        jPanel4 = new javax.swing.JPanel();
        jPanel2 = new javax.swing.JPanel();
        jPanel5 = new javax.swing.JPanel();
        jScroll1 = new javax.swing.JScrollPane();
        tpText = new javax.swing.JTextPane();
        cbDispBin = new javax.swing.JCheckBox();
        btnLoadZones = new javax.swing.JButton();
        btnSaveZones = new javax.swing.JButton();
        btnClearZones = new javax.swing.JButton();
        btnDetectZones = new javax.swing.JButton();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        setTitle("NSOCR - JAVA Advansed Sample");
        setMinimumSize(new java.awt.Dimension(836, 517));
        addWindowListener(new java.awt.event.WindowAdapter() {
            public void windowClosed(java.awt.event.WindowEvent evt) {
                MainFormClossed(evt);
            }
        });

        jPanel3.setBorder(new javax.swing.border.SoftBevelBorder(javax.swing.border.BevelBorder.RAISED));

        btnScan.setText("Scan");
        btnScan.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnScanActionPerformed(evt);
            }
        });

        btnOpenFile.setText("Open File");
        btnOpenFile.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnOpenFileActionPerformed(evt);
            }
        });

        btnRecognize.setText("Recognize");
        btnRecognize.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnRecognizeActionPerformed(evt);
            }
        });

        btnSave.setText("Save");
        btnSave.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnSaveActionPerformed(evt);
            }
        });

        jLabel1.setText("Page");

        tfPage.setText(" ");
        tfPage.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                tfPageActionPerformed(evt);
            }
        });

        jLabel2.setText("of 1");

        btnSetPage.setText("Set");
        btnSetPage.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnSetPageActionPerformed(evt);
            }
        });

        jCheckBox1.setText("Exact copy(do not format text)");
        jCheckBox1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBox1ActionPerformed(evt);
            }
        });

        jLabel3.setText("Sacale:");

        cbScale.setModel(new javax.swing.DefaultComboBoxModel(new String[] { "Auto", "0.25", "0.5", "1.0", "1.5", "2.0", "2.5", "4.0" }));
        cbScale.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                cbScaleActionPerformed(evt);
            }
        });

        jLabel4.setText("Please wait...");

        btnSetLang.setText("Select Language");
        btnSetLang.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnSetLangActionPerformed(evt);
            }
        });

        jButton1.setText("Options");
        jButton1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton1ActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel3Layout = new javax.swing.GroupLayout(jPanel3);
        jPanel3.setLayout(jPanel3Layout);
        jPanel3Layout.setHorizontalGroup(
            jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel3Layout.createSequentialGroup()
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                    .addGroup(jPanel3Layout.createSequentialGroup()
                        .addComponent(btnScan, javax.swing.GroupLayout.PREFERRED_SIZE, 74, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(btnOpenFile))
                    .addGroup(jPanel3Layout.createSequentialGroup()
                        .addComponent(jLabel3)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(cbScale, 0, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(btnRecognize)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(btnSave, javax.swing.GroupLayout.PREFERRED_SIZE, 74, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addComponent(jLabel1)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                    .addGroup(jPanel3Layout.createSequentialGroup()
                        .addComponent(tfPage, javax.swing.GroupLayout.PREFERRED_SIZE, 30, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jLabel2)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                        .addComponent(btnSetPage))
                    .addComponent(jButton1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                .addGap(18, 18, 18)
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanel3Layout.createSequentialGroup()
                        .addComponent(jLabel4)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(jCheckBox1))
                    .addGroup(jPanel3Layout.createSequentialGroup()
                        .addComponent(btnSetLang)
                        .addGap(0, 0, Short.MAX_VALUE)))
                .addContainerGap())
        );
        jPanel3Layout.setVerticalGroup(
            jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
            .addGroup(jPanel3Layout.createSequentialGroup()
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(btnScan)
                    .addComponent(btnOpenFile)
                    .addComponent(btnRecognize)
                    .addComponent(btnSave)
                    .addComponent(jLabel1)
                    .addComponent(tfPage, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jLabel2)
                    .addComponent(btnSetPage)
                    .addComponent(btnSetLang))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jLabel3)
                    .addComponent(cbScale, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jLabel4)
                    .addComponent(jCheckBox1)
                    .addComponent(jButton1))
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );

        jPanel4.setBorder(javax.swing.BorderFactory.createLineBorder(new java.awt.Color(0, 0, 0)));
        jPanel4.setLayout(new java.awt.GridLayout(1, 2));

        jPanel2.addMouseListener(new java.awt.event.MouseAdapter() {
            public void mousePressed(java.awt.event.MouseEvent evt) {
                jPanel2MousePressed(evt);
            }
            public void mouseReleased(java.awt.event.MouseEvent evt) {
                jPanel2MouseReleased(evt);
            }
        });
        jPanel2.addComponentListener(new java.awt.event.ComponentAdapter() {
            public void componentResized(java.awt.event.ComponentEvent evt) {
                jPanel2ComponentResized(evt);
            }
        });
        jPanel2.addMouseMotionListener(new java.awt.event.MouseMotionAdapter() {
            public void mouseDragged(java.awt.event.MouseEvent evt) {
                jPanel2MouseDragged(evt);
            }
            public void mouseMoved(java.awt.event.MouseEvent evt) {
                jPanel2MouseMoved(evt);
            }
        });

        javax.swing.GroupLayout jPanel2Layout = new javax.swing.GroupLayout(jPanel2);
        jPanel2.setLayout(jPanel2Layout);
        jPanel2Layout.setHorizontalGroup(
            jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 417, Short.MAX_VALUE)
        );
        jPanel2Layout.setVerticalGroup(
            jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 411, Short.MAX_VALUE)
        );

        jPanel4.add(jPanel2);

        jPanel5.setLayout(new java.awt.GridLayout(1, 1));

        jScroll1.setViewportView(tpText);

        jPanel5.add(jScroll1);

        jPanel4.add(jPanel5);

        cbDispBin.setText("Display binarized image");
        cbDispBin.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                cbDispBinActionPerformed(evt);
            }
        });

        btnLoadZones.setText("Load Zones");
        btnLoadZones.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnLoadZonesActionPerformed(evt);
            }
        });

        btnSaveZones.setText("Save Zones");
        btnSaveZones.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnSaveZonesActionPerformed(evt);
            }
        });

        btnClearZones.setText("Clear Zones");
        btnClearZones.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnClearZonesActionPerformed(evt);
            }
        });

        btnDetectZones.setText("Detect Zones");
        btnDetectZones.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnDetectZonesActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jPanel3, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
            .addGroup(layout.createSequentialGroup()
                .addComponent(cbDispBin)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(btnLoadZones)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(btnSaveZones)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(btnClearZones)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(btnDetectZones)
                .addGap(0, 0, Short.MAX_VALUE))
            .addComponent(jPanel4, javax.swing.GroupLayout.Alignment.TRAILING, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addComponent(jPanel3, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(1, 1, 1)
                .addComponent(jPanel4, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(cbDispBin)
                    .addComponent(btnLoadZones)
                    .addComponent(btnSaveZones)
                    .addComponent(btnClearZones)
                    .addComponent(btnDetectZones)))
        );

        getAccessibleContext().setAccessibleName("Nicomsoft OCR java Advances Sample");

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void tfPageActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_tfPageActionPerformed
        // TODO add your handling code here:
    }//GEN-LAST:event_tfPageActionPerformed

    private void cbScaleActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_cbScaleActionPerformed
        btnRecognize.setEnabled(false);
	if (IsImgLoaded()) btnOpenFileActionPerformed(evt);       
    }//GEN-LAST:event_cbScaleActionPerformed

    private void MainFormClossed(java.awt.event.WindowEvent evt) {//GEN-FIRST:event_MainFormClossed

    }//GEN-LAST:event_MainFormClossed
 

    private void btnOpenFileActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnOpenFileActionPerformed
            JFileChooser chooser = new JFileChooser();
            
            javax.swing.filechooser.FileNameExtensionFilter filter = 
                    new javax.swing.filechooser.FileNameExtensionFilter(
                      "Image Files (bmp,jpg,tif,png,gif,pdf)", "bmp","jpg","tif","tiff","png","gif","pdf");
            
            chooser.setFileFilter(filter);
            
            int returnVal = chooser.showOpenDialog(this);
            
            if(returnVal != JFileChooser.APPROVE_OPTION) return;
            
            if (cbScale.isEnabled())
            {
                if (cbScale.getSelectedIndex() < 1) //autoscaling
                {
                    NSOCR.Engine.Cfg_SetOption(CfgObj, NSOCR.Constant.BT_DEFAULT, "ImgAlizer/AutoScale", "1");
                    NSOCR.Engine.Cfg_SetOption(CfgObj, NSOCR.Constant.BT_DEFAULT, "ImgAlizer/ScaleFactor", "1.0"); //default scale if cannot detect it automatically
                }
                else //fixed scaling
                {
                    NSOCR.Engine.Cfg_SetOption(CfgObj, NSOCR.Constant.BT_DEFAULT, "ImgAlizer/AutoScale", "0");		
                    NSOCR.Engine.Cfg_SetOption(CfgObj, NSOCR.Constant.BT_DEFAULT, "ImgAlizer/ScaleFactor", cbScale.getSelectedItem().toString());
                }
            }
            
            String Filename = chooser.getSelectedFile().getPath();
            
            int LoadMode = 0; // 0 - from file; 1 - from memory; 2- from raw bitmap
            int res = 0;
            
            if (LoadMode == 0)  // load image to OCR engline from image file
            {   
              res =  NSOCR.Engine.Img_LoadFile(ImgObj, Filename);
            }
            else
                if (LoadMode == 1) //load from image in memory
                {
                  BufferedImage img;

                  try 
                  {
                    img = ImageIO.read(new File(Filename));
                  } 
                  catch (IOException e) 
                  {
                      JOptionPane.showMessageDialog(this, "File not loaded!");
                      return;
                  }

                  ByteArrayOutputStream os = new ByteArrayOutputStream(); 

                  try 
                  {              
                    ImageIO.write(img, "png", os); 
                    os.flush(); 
                  } 
                  catch (IOException e) 
                  {
                      JOptionPane.showMessageDialog(this, "File Not Loaded!");
                      return;
                  }

                  res = NSOCR.Engine.Img_LoadFromMemory(ImgObj, os.toByteArray(), os.size());                            
                }
                else //load from raw bitmap data
                {
                    BufferedImage img;

                    try 
                    {
                        img = ImageIO.read(new File(Filename));
                    } 
                    catch (IOException e) 
                    {
                        JOptionPane.showMessageDialog(this, "File not loaded!");
                        return;
                    }
         
                    int[] rgbArray = new int[img.getWidth() * img.getHeight()];
                    img.getRGB(0, 0, img.getWidth(), img.getHeight(), rgbArray, 0, img.getWidth());
                    
                    res = NSOCR.Engine.Img_LoadBmpData(ImgObj, rgbArray, img.getWidth(), img.getHeight(), NSOCR.Constant.BMP_32BIT);  
                }

            if (res > NSOCR.Error.ERROR_FIRST)
            {
                if (res == NSOCR.Error.ERROR_CANNOTLOADGS) //cannot load GhostScript to support PDF
                {
                     JOptionPane.showMessageDialog(this, "\"GhostSript\" library is needed to support PDF files. Just download and install it with default settings.");
                     return;
                }
                else JOptionPane.showMessageDialog(this, "Error: Img_LoadFile "+Integer.toHexString(res));
                return;
            }
    
            DoImageLoaded();            
    }//GEN-LAST:event_btnOpenFileActionPerformed

    private void btnRecognizeActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnRecognizeActionPerformed
	
        if (!IsImgLoaded())
	{
		JOptionPane.showMessageDialog(this, "Image not loaded!");
		return;
	}

////
	btnRecognize.setEnabled(false);
                
	//m_lbWait.ShowWindow(SW_SHOW);
	btnSaveZones.setEnabled(false);
	this.repaint();

	//perform OCR itself
	int res;
	boolean InSameThread;
	
	InSameThread = false; //perform OCR in non-blocking mode
	//InSameThread = true; //uncomment to perform OCR from this thread (GUI will be freezed)

	if (InSameThread)
	{
		res = NSOCR.Engine.Img_OCR(ImgObj, NSOCR.Constant.OCRSTEP_ZONING, NSOCR.Constant.OCRSTEP_LAST, NSOCR.Constant.OCRFLAG_NONE);                
	}
	else
	{
            //do it in non-blocking mode and then wait for result
            res = NSOCR.Engine.Img_OCR(ImgObj, NSOCR.Constant.OCRSTEP_ZONING, NSOCR.Constant.OCRSTEP_LAST, NSOCR.Constant.OCRFLAG_THREAD);
	    if (res > NSOCR.Error.ERROR_FIRST)
	    {
                JOptionPane.showMessageDialog(this, "Ocr_OcrImg(1)" + Integer.toHexString(res));
                return;
            }
            
	  dlgWait dlg = new dlgWait(this, true);
	  dlg.init(ImgObj, 0);
          dlg.setLocationRelativeTo(this);
	  dlg.setVisible(true);
	  res = dlg.res;

	}

	if (res > NSOCR.Error.ERROR_FIRST)
	{
		if (res == NSOCR.Error.ERROR_OPERATIONCANCELLED)
                    JOptionPane.showMessageDialog(this, "Operation was cancelled.");
		else
		{
			JOptionPane.showMessageDialog(this, "Img_OCR " + Integer.toHexString(res));
			return;
		}
	}
    
        //m_lbWait.ShowWindow(SW_HIDE); 
	btnRecognize.setEnabled(true);
	btnSaveZones.setEnabled(true);
        btnSave.setEnabled(true);
        
   //     HBLK BlkObj = new HBLK();
   //     NSOCR.Engine.Img_GetBlock(ImgObj, 0, BlkObj);
   //     NSOCR.Engine.Blk_SetWordText(BlkObj, 0, 0, "usertext");
////	
	AdjustDocScale(); //repaint img (binarized image could change)	
	ShowText();        // TODO add your handling code here:
    }//GEN-LAST:event_btnRecognizeActionPerformed

    private void jCheckBox1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBox1ActionPerformed
        ShowText();
    }//GEN-LAST:event_jCheckBox1ActionPerformed

    private void jPanel2ComponentResized(java.awt.event.ComponentEvent evt) {//GEN-FIRST:event_jPanel2ComponentResized
        // TODO add your handling code here:
        AdjustDocScale();
    }//GEN-LAST:event_jPanel2ComponentResized

    private void cbDispBinActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_cbDispBinActionPerformed
        // TODO add your handling code here:
        AdjustDocScale();
    }//GEN-LAST:event_cbDispBinActionPerformed

    private void jPanel2MousePressed(java.awt.event.MouseEvent evt) {//GEN-FIRST:event_jPanel2MousePressed
        // TODO add your handling code here:
        if (!IsImgLoaded()) return;
        
        Rect r   = new Rect();
        r.left   = 0;
        r.top    = 0;
        r.right  = jPanel2.getWidth()-1;
        r.bottom = jPanel2.getHeight()-1;

        int   MouseX = evt.getX();
        int   MouseY = evt.getY();
        float k      = GetCurDocScale();        
        
	if (evt.getButton() == MouseEvent.BUTTON1)
        {            
            if (r.PtInRect(MouseX, MouseY))
            {
		NSInt w, h;
                w = new NSInt(0);
                h = new NSInt(0);
                
		NSOCR.Engine.Img_GetSize(ImgObj, w, h);

		Dwn = true;
                
		Frame.left = (int)(1 / k * (MouseX - r.left));
		if (Frame.left < 0) Frame.left = 0;
		if (Frame.left > w.Value) Frame.left = w.Value;
		Frame.top = (int)(1 / k * (MouseY - r.top));
		if (Frame.top < 0) Frame.top = 0;
		if (Frame.top > h.Value) Frame.top = h.Value;

		Frame.right = Frame.left;
		Frame.bottom = Frame.top;

		ShowImage();
            }
	}  
        
	if (evt.getButton() == MouseEvent.BUTTON3)
        {
            if (r.PtInRect(MouseX, MouseY))
            {
                int cnt = NSOCR.Engine.Img_GetBlockCnt(ImgObj);
                
                int n = -1;
                int sz, minsize = -1;
                
                HBLK BlkObj  = new HBLK();
                NSInt Xpos   = new NSInt(0);
                NSInt Ypos   = new NSInt(0);
                NSInt Width  = new NSInt(0);
                NSInt Height = new NSInt(0); 
                
		Rect rb = new Rect();
                
                for (int i = 0; i < cnt; i++)
                {
                    NSOCR.Engine.Img_GetBlock(ImgObj, i, BlkObj);
                    NSOCR.Engine.Blk_GetRect(BlkObj, Xpos, Ypos, Width, Height);
                    
                    rb.left   = (int)(k *  Xpos.Value);
                    rb.top    = (int)(k *  Ypos.Value);
                    rb.right  = (int)(k * (Xpos.Value + Width.Value - 1));
                    rb.bottom = (int)(k * (Ypos.Value + Height.Value - 1));
            
                    if (rb.PtInRect(MouseX, MouseY))
                    {
                        //need to find smallest block because blocks may overlap
                        if (Width.Value < Height.Value) sz = Width.Value;
                        else sz = Height.Value;

                        if ((minsize == -1) || (sz < minsize))
                        {
                            minsize = sz;
                            n = i;
                        }
                    }
                }

                if (n == - 1) return; //block not found
                pmBlockTag = n; //remember block index              
       
                NSOCR.Engine.Img_GetBlock(ImgObj, pmBlockTag, BlkObj);
                
                BlockTypePopUp popupmenu = new BlockTypePopUp(ImgObj, BlkObj, this);
                popupmenu.Show(jPanel2, MouseX, MouseY);
            }            
        }        
    }//GEN-LAST:event_jPanel2MousePressed

    private void jPanel2MouseReleased(java.awt.event.MouseEvent evt) {//GEN-FIRST:event_jPanel2MouseReleased
        // TODO add your handling code here:
        if (!IsImgLoaded()) return;
    
        HBLK BlkObj = new HBLK();
        NSInt w = new NSInt(0);
        NSInt h = new NSInt(0);
        int res;
    
        if (!Dwn) return;
        Dwn = false;

        NSOCR.Engine.Img_GetSize(ImgObj, w, h);
        if (Frame.right >= w.Value) Frame.right = w.Value - 1;
        if (Frame.bottom >= h.Value) Frame.bottom = h.Value - 1;

        w.Value = Frame.right - Frame.left + 1;
        h.Value = Frame.bottom - Frame.top + 1;
    
        if ((w.Value < 8) || (h.Value < 8))
        {
            ShowImage();
            return;
        }
    
        res = NSOCR.Engine.Img_AddBlock(ImgObj, Frame.left, Frame.top, w.Value, h.Value, BlkObj);
    
        if (res > NSOCR.Error.ERROR_FIRST)
        {
            JOptionPane.showMessageDialog(this, "Img_AddBlock" + Integer.toHexString(res));
            return;
        }

        //detect text block inversion
        NSOCR.Engine.Blk_Inversion(BlkObj, NSOCR.Constant.BLK_INVERSE_DETECT);
        //detect text block rotation
        NSOCR.Engine.Blk_Rotation(BlkObj, NSOCR.Constant.BLK_ROTATE_DETECT);        

        ShowImage();        
    }//GEN-LAST:event_jPanel2MouseReleased

    private void jPanel2MouseMoved(java.awt.event.MouseEvent evt) {//GEN-FIRST:event_jPanel2MouseMoved
	

    }//GEN-LAST:event_jPanel2MouseMoved

    private void jPanel2MouseDragged(java.awt.event.MouseEvent evt) {//GEN-FIRST:event_jPanel2MouseDragged
        if (!IsImgLoaded()) return;
        if (!Dwn) return;
	
	Rect r = new Rect();
        
        r.left = 0;
        r.top  = 0;
        r.right  = jPanel2.getWidth()-1;
        r.bottom = jPanel2.getHeight()-1;
        
        int MouseX = evt.getX();
        int MouseY = evt.getY();

	if (r.PtInRect(MouseX, MouseY))
	{
		NSInt w = new NSInt(0);
                NSInt h = new NSInt(0);
                
		NSOCR.Engine.Img_GetSize(ImgObj, w, h);

		float k = GetCurDocScale();
                
		Frame.right = (int)(1 / k * (MouseX - r.left));
		if (Frame.right < 0) Frame.right = 0;
		if (Frame.right > w.Value) Frame.right = w.Value;
                Frame.bottom = (int)(1 / k * (MouseY - r.top));
		if (Frame.bottom < 0) Frame.bottom = 0;
		if (Frame.bottom > h.Value) Frame.bottom = h.Value;

		ShowImage();
	} 
    }//GEN-LAST:event_jPanel2MouseDragged

    private void btnSaveActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnSaveActionPerformed
	
        SaveDocument();
    }//GEN-LAST:event_btnSaveActionPerformed

    private void btnLoadZonesActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnLoadZonesActionPerformed

        JFileChooser chooser = new JFileChooser();
            
        javax.swing.filechooser.FileNameExtensionFilter filter = 
            new javax.swing.filechooser.FileNameExtensionFilter("blk files", "bmp","blk");
            
         chooser.setFileFilter(filter);
         int returnVal = chooser.showOpenDialog(this);
            
        if(returnVal != JFileChooser.APPROVE_OPTION) return;

        NSOCR.Engine.Img_DeleteAllBlocks(ImgObj); //note: Img_LoadBlocks does not remove existing blocks, so remove them here
	btnSave.setEnabled(false);
        
        int res = NSOCR.Engine.Img_LoadBlocks(ImgObj, chooser.getSelectedFile().getAbsolutePath());
        
        if (res > NSOCR.Error.ERROR_FIRST)
        {
            JOptionPane.showMessageDialog(this, "Img_LoadBlocks "+Integer.toHexString(res));
            return;
        }
        
        ShowImage();
    }//GEN-LAST:event_btnLoadZonesActionPerformed

    private void btnSaveZonesActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnSaveZonesActionPerformed
        
        JFileChooser chooser = new JFileChooser();
        javax.swing.filechooser.FileNameExtensionFilter filter;
                    
        filter = new javax.swing.filechooser.FileNameExtensionFilter("blk files", "blk");
        chooser.setFileFilter(filter); 
            
        chooser.setAcceptAllFileFilterUsed(false);
        int returnVal = chooser.showSaveDialog(this);        

        if (returnVal != JFileChooser.APPROVE_OPTION) return;
        
	int res = NSOCR.Engine.Img_SaveBlocks(ImgObj, chooser.getSelectedFile().getAbsolutePath());
	if (res > NSOCR.Error.ERROR_FIRST) 
            JOptionPane.showMessageDialog(this, "Img_SaveBlocks "+Integer.toHexString(res));
    }//GEN-LAST:event_btnSaveZonesActionPerformed

    private void btnClearZonesActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnClearZonesActionPerformed
	NSOCR.Engine.Img_DeleteAllBlocks(ImgObj);
	btnSave.setEnabled(false);
	ShowImage();
    }//GEN-LAST:event_btnClearZonesActionPerformed

    private void btnDetectZonesActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnDetectZonesActionPerformed
	NSOCR.Engine.Img_DeleteAllBlocks(ImgObj);
	NSOCR.Engine.Img_OCR(ImgObj, NSOCR.Constant.OCRSTEP_ZONING, NSOCR.Constant.OCRSTEP_ZONING, NSOCR.Constant.OCRFLAG_NONE);
	ShowImage();
    }//GEN-LAST:event_btnDetectZonesActionPerformed

    private void btnSetPageActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnSetPageActionPerformed
	
        if (!IsImgLoaded()) return;
        
	int cnt = NSOCR.Engine.Img_GetPageCount(ImgObj);
        String page = tfPage.getText();
         
	int n = Integer.parseInt(page) - 1;
	if (n < 0) n = 0;
	if (n >= cnt) n = cnt -1;
        
	NSOCR.Engine.Img_SetPage(ImgObj, n);
        tfPage.setText(String.valueOf(n+1)); 
	btnSave.setEnabled(false);
        
	//now apply image scaling, binarize image, deskew etc,
	//everything except OCR itself
	int res = NSOCR.Engine.Img_OCR(ImgObj, NSOCR.Constant.OCRSTEP_FIRST, NSOCR.Constant.OCRSTEP_ZONING - 1, NSOCR.Constant.OCRFLAG_NONE);
	if (res > NSOCR.Error.ERROR_FIRST) 
            JOptionPane.showMessageDialog(this, "Img_OCR" + Integer.toHexString(res));

	AdjustDocScale();        // TODO add your handling code here:
    }//GEN-LAST:event_btnSetPageActionPerformed

    private void btnScanActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnScanActionPerformed
        Scan DLG = new Scan(this, true);
        DLG.Init(ScanObj, ImgObj);
        DLG.setLocationRelativeTo(this);
        
        if (DLG.EnumOK) DLG.setVisible(true);        
        if (DLG.isOk) DoImageLoaded();       
    }//GEN-LAST:event_btnScanActionPerformed

    private void btnSetLangActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnSetLangActionPerformed
        LangDlg dlg = new LangDlg(this, true);
        dlg.setLocationRelativeTo(this);
        dlg.init(CfgObj);
        dlg.setVisible(true);
    }//GEN-LAST:event_btnSetLangActionPerformed

    private void jButton1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton1ActionPerformed
               
        wOptions DLG = new wOptions(this, true);
        DLG.Init(CfgObj);
        DLG.setLocationRelativeTo(this);
        
        DLG.setVisible(true);
    }//GEN-LAST:event_jButton1ActionPerformed

    /**
     * @param args the command line arguments
     */
    public static void main(String args[]) {
        /* Set the Nimbus look and feel */
        //<editor-fold defaultstate="collapsed" desc=" Look and feel setting code (optional) ">
        /* If Nimbus (introduced in Java SE 6) is not available, stay with the default look and feel.
         * For details see http://download.oracle.com/javase/tutorial/uiswing/lookandfeel/plaf.html 
         */
        try {
            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (ClassNotFoundException ex) {
            java.util.logging.Logger.getLogger(MainFrame.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            java.util.logging.Logger.getLogger(MainFrame.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            java.util.logging.Logger.getLogger(MainFrame.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(MainFrame.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(new Runnable() {
            public void run() {
                new MainFrame().setVisible(true);
            }
        });
    }
    private HCFG    CfgObj;
    private HIMG    ImgObj;
    private HOCR    OcrObj;
    private HSCAN   ScanObj;
    private HSVR    SvrObj;
    private boolean Dwn;
    private boolean IsProcessPagesMode;
    private BufferedImage bmp;
    private nsDrawPanel DocImg;
    private Rect    Frame;
    private String  SavedFileName;
    public  int     pmBlockTag;
    
    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton btnClearZones;
    private javax.swing.JButton btnDetectZones;
    private javax.swing.JButton btnLoadZones;
    private javax.swing.JButton btnOpenFile;
    private javax.swing.JButton btnRecognize;
    private javax.swing.JButton btnSave;
    private javax.swing.JButton btnSaveZones;
    private javax.swing.JButton btnScan;
    private javax.swing.JButton btnSetLang;
    private javax.swing.JButton btnSetPage;
    private javax.swing.JCheckBox cbDispBin;
    private javax.swing.JComboBox cbScale;
    private javax.swing.JButton jButton1;
    private javax.swing.JCheckBox jCheckBox1;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JLabel jLabel4;
    private javax.swing.JPanel jPanel2;
    private javax.swing.JPanel jPanel3;
    private javax.swing.JPanel jPanel4;
    private javax.swing.JPanel jPanel5;
    private javax.swing.JScrollPane jScroll1;
    private javax.swing.JTextField tfPage;
    private javax.swing.JTextPane tpText;
    // End of variables declaration//GEN-END:variables
}
