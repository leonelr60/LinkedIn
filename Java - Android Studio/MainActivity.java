package com.litd.edgar.teacherassistant;
import android.Manifest;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.ActivityNotFoundException;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Environment;
import android.print.PrintAttributes;
import android.print.PrintDocumentAdapter;
import android.print.PrintJob;
import android.print.PrintManager;
import android.support.v4.app.ActivityCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.webkit.JavascriptInterface;
import android.webkit.WebChromeClient;
import android.webkit.WebResourceError;
import android.webkit.WebResourceRequest;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.ImageButton;
import android.widget.Toast;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;

public class MainActivity extends AppCompatActivity {

    private static final int REQUEST_CODE_PERMISSION = 2;
    private static final String[] PERMISSIONS_REQ = {
            Manifest.permission.WRITE_EXTERNAL_STORAGE
    };
    public static void verifyPermissions(Activity activity) {
        // Check if we have write permission
        int WritePermision = ActivityCompat.checkSelfPermission(activity, Manifest.permission.WRITE_EXTERNAL_STORAGE);
        if (WritePermision != PackageManager.PERMISSION_GRANTED) {
            // We don't have permission so prompt the user
            ActivityCompat.requestPermissions(
                    activity,
                    PERMISSIONS_REQ,
                    REQUEST_CODE_PERMISSION
            );

        }
    }
    @SuppressLint("SetJavaScriptEnabled")
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        verifyPermissions(this);
        WebView myWebView = (WebView) findViewById(R.id.wvApp);
        ImageButton myPrintingButton = (ImageButton) findViewById(R.id.button);
        myWebView.setWebChromeClient(new WebChromeClient());
        WebSettings webSettings = myWebView.getSettings();
        webSettings.setJavaScriptEnabled(true);
        webSettings.setAllowUniversalAccessFromFileURLs(true);
        webSettings.setJavaScriptCanOpenWindowsAutomatically(true);
        webSettings.setSupportMultipleWindows(true);
        webSettings.setDomStorageEnabled(true);
        webSettings.setAppCacheEnabled(true);
        webSettings.setUseWideViewPort(true);
        webSettings.setLoadWithOverviewMode(true);
        webSettings.setAllowFileAccessFromFileURLs(true);
        myWebView.getSettings().setMediaPlaybackRequiresUserGesture(false);
        myWebView.requestFocusFromTouch();
        myWebView.setWebViewClient(new WebViewClientImpl(this));
        myWebView.addJavascriptInterface(new WebAppInterface(this), "Android");
        myPrintingButton.setVisibility(View.GONE);
        if (savedInstanceState == null) {
            myWebView.loadUrl("file:///android_asset/index.html");

        }

    }
    public PrintJob printJob;
    public boolean printBtnPressed = false;

    public void createWebPrintJob(WebView webView) {

        PrintManager printManager = (PrintManager) this.getSystemService(Context.PRINT_SERVICE);

        PrintDocumentAdapter printAdapter =
                webView.createPrintDocumentAdapter("MyDocument");

        String jobName = getString(R.string.app_name) + " Print Session";

        printJob = printManager.print(jobName, printAdapter,
                new PrintAttributes.Builder().build());
    }

    public void WebPrintJob(View view) {
        printBtnPressed = true;
        WebView myWebView = (WebView) findViewById(R.id.wvApp);
        createWebPrintJob(myWebView);
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (printJob != null && printBtnPressed) {
            if (printJob.isCompleted()) {
                // Showing Toast Message
                Toast.makeText(this, "Completed", Toast.LENGTH_SHORT).show();
            } else if (printJob.isStarted()) {
                // Showing Toast Message
                Toast.makeText(this, "isStarted", Toast.LENGTH_SHORT).show();

            } else if (printJob.isBlocked()) {
                // Showing Toast Message
                Toast.makeText(this, "isBlocked", Toast.LENGTH_SHORT).show();

            } else if (printJob.isCancelled()) {
                // Showing Toast Message
                Toast.makeText(this, "isCancelled", Toast.LENGTH_SHORT).show();

            } else if (printJob.isFailed()) {
                // Showing Toast Message
                Toast.makeText(this, "isFailed", Toast.LENGTH_SHORT).show();

            } else if (printJob.isQueued()) {
                // Showing Toast Message
                Toast.makeText(this, "isQueued", Toast.LENGTH_SHORT).show();

            }
            // set printBtnPressed false
            printBtnPressed = false;
        }
    }

    //Interface for toast processes in Java
    public class WebAppInterface {
        Context mContext;

        /** Instantiate the interface and set the context */
        WebAppInterface(Context c) {
            mContext = c;
        }

        /** Show a toast from the web page */
        @JavascriptInterface
        public void showToast(String toast) {
            if(toast.startsWith("MKFILE"))
            {
                //Save file
                String sType = toast.substring(0,10);

                toast = toast.substring(10);
                saveCSV(toast,sType);
            }
            else
            {
                if(toast.startsWith("MKPRINTDOC"))
                {
                    //printing process
                    Toast.makeText(mContext, "Document ready to print...", Toast.LENGTH_SHORT).show();
                }
                else
                {
                    Toast.makeText(mContext, toast, Toast.LENGTH_SHORT).show();
                }

            }

        }
    }

    public void saveCSV(String data, String sType)
    {
        // Get the directory for the user's public downloads directory.
        final File path =
                Environment.getExternalStoragePublicDirectory
                        (
                                Environment.DIRECTORY_DOWNLOADS
                        );

        // Make sure the path directory exists.
        if(!path.exists())
        {
            // Make it, if it doesn't exit
            //noinspection ResultOfMethodCallIgnored
            path.mkdirs();
        }

        String sFileName = "/downloaded.txt";
        if(sType.contains("MKFILESTUD"))
        {
            sFileName = "/ReporteEstudiantes.csv";
        }
        if(sType.contains("MKFILENOTE"))
        {
            sFileName = "/ReporteNotas.csv";
        }
        if(sType.contains("MKFILEACTI"))
        {
            sFileName = "/ReporteActividades.csv";
        }
        final File file = new File(path, sFileName);

        try
        {
            //noinspection ResultOfMethodCallIgnored
            file.createNewFile();
            FileOutputStream fOut = new FileOutputStream(file);
            OutputStreamWriter myOutWriter = new OutputStreamWriter(fOut);
            myOutWriter.append(data);
            myOutWriter.close();
            fOut.flush();
            fOut.close();
            Toast.makeText(this, "File generated, check your Downloads Folder...", Toast.LENGTH_SHORT).show();
            File objfile = new File(path+sFileName);
            Intent intent = new Intent(Intent.ACTION_VIEW);
            intent.setDataAndType(Uri.fromFile(objfile) , "text/csv");
            intent.setFlags(Intent.FLAG_ACTIVITY_NO_HISTORY);
            startActivity(intent);

        }
        catch (IOException e)
        {
            Log.e("Exception", "File write failed: " + e.toString());
            Toast.makeText(this, e.toString(), Toast.LENGTH_SHORT).show();
        }
    }

    public class WebViewClientImpl extends WebViewClient {

        private final Context activity;


        @Override
        public void onPageFinished(WebView view, String url)
        {

        }

        @Override
        public void onPageStarted(WebView view, String url, Bitmap favicon) {
            // TODO Auto-generated method stub
            super.onPageStarted(view, url, favicon);

        }

        @Override
        public boolean shouldOverrideUrlLoading(WebView view, WebResourceRequest request)
        {
            ImageButton myPrintingButton = (ImageButton) findViewById(R.id.button);
            myPrintingButton.setVisibility(View.GONE);
            String url = request.getUrl().toString();
            if (url.startsWith("tel:")) {
                Intent intent = new Intent(Intent.ACTION_DIAL,
                        Uri.parse(url));
                startActivity(intent);

            }
            if (url.startsWith("mailto:")) {
                Intent intent = new Intent(Intent.ACTION_SENDTO,
                        Uri.parse(url));
                startActivity(intent);

            }

            if(url.contains("activitiesreports") || url.contains("notesreports") || url.contains("studentsreports")){

                myPrintingButton.setVisibility(View.VISIBLE);
            }

            if(url.contains(".pdf") || url.contains(".mp4") || url.contains(".csv") )
            {

                String sFile = null;
                String sType = "application/pdf";

                if(url.contains(".mp4")) {
                    sType= "video/mp4";
                }
                if(url.contains(".csv")) {
                    sType= "text/csv";
                }
                File file = new File(sFile);
                Uri path = Uri.fromFile(file);
                Intent intent = new Intent(Intent.ACTION_VIEW);
                intent.setDataAndType(path, sType);
                intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                System.out.println("Done!" + file.getAbsolutePath());
                try {
                    activity.startActivity(intent);
                } catch (ActivityNotFoundException f) {
                    Toast.makeText(activity, "No application available to display the file", Toast.LENGTH_LONG).show();
                }

            }
            else {
                view.loadUrl(url);
            }
            return true;
        }

        /*
        @Override
        public boolean shouldOverrideUrlLoading(WebView view, String url) {
            // TODO Auto-generated method stub
            ImageButton myPrintingButton = (ImageButton) findViewById(R.id.button);
            myPrintingButton.setVisibility(View.GONE);
            if (url.startsWith("tel:")) {
                Intent intent = new Intent(Intent.ACTION_DIAL,
                        Uri.parse(url));
                startActivity(intent);

            }
            if (url.startsWith("mailto:")) {
                Intent intent = new Intent(Intent.ACTION_SENDTO,
                        Uri.parse(url));
                startActivity(intent);

            }

            if(url.contains("activitiesreports") || url.contains("notesreports") || url.contains("studentsreports")){

                myPrintingButton.setVisibility(View.VISIBLE);
            }

            if(url.contains(".pdf") || url.contains(".mp4") || url.contains(".csv") )
            {

                String sFile = null;
                String sType = "application/pdf";

                if(url.contains(".mp4")) {
                    sType= "video/mp4";
                }
                if(url.contains(".csv")) {
                    sType= "text/csv";
                }
                File file = new File(sFile);
                Uri path = Uri.fromFile(file);
                Intent intent = new Intent(Intent.ACTION_VIEW);
                intent.setDataAndType(path, sType);
                intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                System.out.println("Done!" + file.getAbsolutePath());
                try {
                    activity.startActivity(intent);
                } catch (ActivityNotFoundException f) {
                    Toast.makeText(activity, "No application available to display the file", Toast.LENGTH_LONG).show();
                }

            }
            else {
                view.loadUrl(url);
            }
            return true;

        }*/

        @Override
        public void onReceivedError(WebView view, WebResourceRequest request, WebResourceError error){
            String sMsg;
            if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.M) {
                if(view.getUrl().contains("mailto:") || view.getUrl().contains("tel:"))
                {
                    sMsg = "Loading...";
                }
                else {
                    sMsg = error.getDescription().toString();
                }

            }else
            {
                sMsg = error.toString();
            }

            if(sMsg.contains("ERR_NAME_NOT_RESOLVED") ) {
                sMsg = "Please check your Internet Connection...";
            }

            if(sMsg.contains("ERR_UNKNOWN_URL_SCHEME") ) {
                sMsg = "Loading...";
            }

            if(sMsg.contains("ERR_FILE_NOT_FOUND") ) {
                return;
            }

            Toast.makeText(activity, sMsg , Toast.LENGTH_LONG).show();

                view.goBack();


        }

        public WebViewClientImpl(Context activityp){
            activity = activityp;
        }


    }

    @Override
    protected void onSaveInstanceState(Bundle outState )
    {
        super.onSaveInstanceState(outState);
        WebView myWebView = (WebView) findViewById(R.id.wvApp);
        myWebView.saveState(outState);
    }

    @Override
    protected void onRestoreInstanceState(Bundle savedInstanceState)
    {
        super.onRestoreInstanceState(savedInstanceState);
        WebView myWebView = (WebView) findViewById(R.id.wvApp);
        myWebView.restoreState(savedInstanceState);
    }

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        ImageButton myPrintingButton = (ImageButton) findViewById(R.id.button);
        myPrintingButton.setVisibility(View.GONE);
        WebView myWebView = (WebView) findViewById(R.id.wvApp);
        if (event.getAction() == KeyEvent.ACTION_DOWN) {
            if (keyCode == KeyEvent.KEYCODE_BACK) {
                if (myWebView.canGoBack()) {
                    if (myWebView.getUrl().contains("studentsreports") || myWebView.getUrl().contains("notesreports") || myWebView.getUrl().contains("activitiesreports")) {
                        myPrintingButton.setVisibility(View.GONE);
                    }
                    if (myWebView.getUrl().contains("index")) {
                        finish();
                    } else {
                        myWebView.goBack();
                    }

                } else {
                    finish();
                }
                return true;
            }

        }
        return super.onKeyDown(keyCode, event);
    }



}


