/*
Gabriel Paquette
October 8, 2016
This application is a basic text editor, which allows the user
the open and save text documents.
*/


/*
 * File         : MainWindow.xaml.cs
 * Project      : Text Editor
 * Developer(s) : Gabriel Paquette
 * Date Created : 2016-10-09
 * Description  : This application is a basic text editor, which allows the user
 *                the open and save text documents.
 */
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.IO;

namespace Text_Editor
{
   
    public partial class MainWindow : Window
    {

        private static bool fileSaved; //true if the file and saved
                                       //false if the file is not saved

        /*
        Name:MainWindow
        Description: initializes the application as well as the stats shown on screen
        */
        public MainWindow()
        {
            InitializeComponent();

            initalizeTextData();
        }

        /*
        Name:Text_EditorWindow_Closing
        Description:This method gets called when the window is close
                    or when this.close() is called, it verifies if the file 
                    is saved, and asks the user if they would like to save it 
                    before closing
        */
        private void Text_EditorWindow_Closing(object sender, CancelEventArgs e)
        {
            //if the file is saved, then close
            if (fileSaved == false)
            {
                //if the file is not saved, ask if they 
                //want to save it
                if (getSaveConfirmation() == true)
                {
                    //if they do want to save it
                    //and it fails to save, dont close
                    if (saveFile() == false)
                    {
                        //cancel the window close
                        e.Cancel = true;
                    }
                }
            }
        }


        /*
        Name:initalizeTextData
        Description: this method initalizes the character count and the curser index */
        private void initalizeTextData()
        {
            fileSaved = true;

            //initalizes the curser index and the total number of characters.
            indexPosition.Text = "Line " + 1 + ", Char " + 1;
            charCount.Text = "Total Character: " + 0;

            //sets the focus of the computer and keyboard to the textbox
            FocusManager.SetFocusedElement(this, txtEditor);
            Keyboard.Focus(txtEditor);
            
        }


        /*
        Name: txtEditor_SelectionChanged
        Description: this method updates when the text box is interacted with. It
                     updates the character count and the curser index
        */
        private void txtEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //gets the correct row and column for the curser index.
            int row = txtEditor.GetLineIndexFromCharacterIndex(txtEditor.CaretIndex);
            int col = txtEditor.CaretIndex - txtEditor.GetCharacterIndexFromLineIndex(row);

            //formats the curser index into readable information
            indexPosition.Text = "Line " + (row + 1) + ", Char " + (col + 1);

            //this gets the total number of chacters in the text box, and returns it
            //it also replaces any new lines to " ", this is because newlines register as
            //two characters \r\n.
            charCount.Text = "Total Character: " + txtEditor.Text.Replace(Environment.NewLine, " ").Length;
            fileSaved = false;
        }


        /*
        Name:getSaveConfirmation
        Description: This asks the user if they want to save the text, if they click yes
                     then it returns true, it returns false if they click no
        Return: save-> true if they want to save and false if they dont
        */
        private bool getSaveConfirmation()
        {

            bool save = false;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you want to save the changes you've made?", "Save Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                save = true;
            }

            return save;
        }


        /*
        Name:menuClose_Click
        Description: this is called then the user clicks the close option 
                     in the file menu
        */
        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            //this calls Text_EditorWindow_Closing where the close is handled
            this.Close();
        }


        /*
        Name:menuOpen_Click
        Description: this method is called then the user clicks the open
                     option in the file menu. Before opening a file, it
                     checks if the current file is saved. it its not, it 
                     checks if the user wants to save it.
        */
        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            //if the file is not saved
            if (fileSaved == false)
            {
                //ask if they want to save it
                //if they don't want to save it
                if (getSaveConfirmation() == false)
                {
                    openFile();
                    
                }
                else
                {
                    //if the save worked, then open the new file
                    if (saveFile() == true)
                    {
                        openFile();
                    }
                }
            }
            else
            {
                openFile();
            }
        }


        /*
        Name:saveFile
        Description: saves the current text in the text box to a file
                     using the save file dialog box
        Return: true if the save worked, and false if it failed*/
        private bool saveFile()
        {

            bool saveSuccessful = false;
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = "Document"; // Default file name
            sfd.DefaultExt = ".txt"; // Default file extension
            sfd.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = sfd.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                string fileName = sfd.FileName;
                StreamWriter file = new StreamWriter(fileName);

                file.WriteLine(txtEditor.Text);
 
                file.Close();
                fileSaved = true;
                saveSuccessful = true;
            }

            return saveSuccessful;
        }


        /*
        Name: openFile
        Description: opens a text file using the open file dialog box*/
        private void openFile()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "Document"; // Default file name
            ofd.DefaultExt = ".txt"; // Default file extension
            ofd.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = ofd.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                string fileName = ofd.FileName;
                StreamReader file = new StreamReader(fileName);
                txtEditor.Text = file.ReadToEnd();
                file.Close();
                fileSaved = true;
            }
        }


        /*
        Name: menuSaveAs_Click 
        Description: this method is called then the close option in the file
                     menu is clicked.
        */
        private void menuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            saveFile();
        }


        /*
        Name: menuNew_Click
        Description: this method asked the use if they want to save the current text
                     before clearning the text
        */
        private void menuNew_Click(object sender, RoutedEventArgs e)
        {
            if (fileSaved == false)
            {
                //if they didnt want to save
                if (getSaveConfirmation() == false)
                {
                    txtEditor.Text = "";
                    fileSaved = true;
                }
                //if they did want to save
                else
                {
                    //if the file was saved properly
                    if (saveFile() == true)
                    {
                        txtEditor.Text = "";
                        fileSaved = true;
                    }
                }
            }
            else
            {
                //if the file is already saved
                txtEditor.Text = "";
                fileSaved = true;
            }
        }


        /*
        Name:menuAbout_Click
        Description: this method is called then the user clicks the about option
                     it gives a brief description about the program and who made it
        */
        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            //shows information about the current version of the program
            MessageBox.Show("Developed by: Gabriel Paquette\nFirst Updated: October 4th, 2016 - Version 1.0\nLast Updated: October 6th, 2016 - Version 1.1", "Program Information");
                             
        }


        /*
        Name:menuTextWrap_Click
        Description: this method is called when the user clicks the text wrap option 
                     in the options menu. it enables the disables the text wrap feature
        */
        private void menuTextWrap_Click(object sender, RoutedEventArgs e)
        {
            if(txtEditor.TextWrapping == TextWrapping.Wrap)
            {
                txtEditor.TextWrapping = TextWrapping.NoWrap;
            }
            else
            {
                txtEditor.TextWrapping = TextWrapping.Wrap;
            }
        }


        /*
        Name:changeFont
        Parameters:int fontType ->indicates what kind if font the user wants
        Description: Changes the font type of the text file*/
        private void changeFont(int fontType)
        {
            //unchecks all of the font options
            menuArialFont.IsChecked = false;
            menuDefaultFont.IsChecked = false;
            menuCalibriFont.IsChecked = false;
            menuTNRFont.IsChecked = false;

            string newFontType = "";

            //sets the requested type of font and checks it in the font menu
            switch (fontType)
            {
                case 0:
                    newFontType = "Consolas";
                    menuDefaultFont.IsChecked = true;
                    break;
                case 1:
                    newFontType = "Times New Roman";
                    menuTNRFont.IsChecked = true;
                    break;
                case 2:
                    newFontType = "Arial";
                    menuArialFont.IsChecked = true;
                    break;
                case 3:
                    newFontType = "Calibri";
                    menuCalibriFont.IsChecked = true;
                    break;
            }
            //set the font
            txtEditor.FontFamily = new FontFamily(newFontType );
        }


        /*
        Name:menuTNRFont_Click
        Description: indicates what font to change to
        */
        private void menuDefaultFont_Click(object sender, RoutedEventArgs e)
        {
            changeFont(0);
        }



        /*
        Name:
        Description: indicates what font to change to
        */
        private void menuTNRFont_Click(object sender, RoutedEventArgs e)
        {
            changeFont(1);
        }



        /*
        Name:menuArialFont_Click
        Description: indicates what font to change to
        */
        private void menuArialFont_Click(object sender, RoutedEventArgs e)
        {
            changeFont(2);
        }



        /*
        Name:menuCalibriFont_Click
        Description: indicates what font to change to
        */
        private void menuCalibriFont_Click(object sender, RoutedEventArgs e)
        {
            changeFont(3);
        }
    }
}
