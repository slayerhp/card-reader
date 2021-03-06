﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace card_reader
{
    public partial class FormChangeInfoByCardNumber : Form
    {
        public int cardId { get; set; }
        public string loginCard { get; set; }
        public string ip { get; set; }
        private static string APP_PATH = "http://localhost:9000";
        public FormChangeInfoByCardNumber()
        {
            InitializeComponent();
        }
        public FormChangeInfoByCardNumber(int cardId, string loginCard, string ip)
        {
            this.cardId = cardId;
            this.loginCard = loginCard;
            this.ip = ip;
            InitializeComponent();
            formLoad(cardId, loginCard, ip);
            APP_PATH = ConfigurationManager.AppSettings.Get("serverURI");
        }
        private List<Button> buttonsDeleteNew = new List<Button>();
        private List<RichTextBox> richTextBoxesFIOchildrenNew = new List<RichTextBox>();
        private List<MaskedTextBox> maskedTextBoxesChildrenNew = new List<MaskedTextBox>();
        private List<ComboBox> comboBoxesesChildrenNew = new List<ComboBox>();
        private List<Button> buttonsDeleteExist = new List<Button>();
        private List<RichTextBox> richTextBoxesFIOchildrenExsist = new List<RichTextBox>();
        private List<MaskedTextBox> maskedTextBoxesChildrenExsist = new List<MaskedTextBox>();
        private List<ComboBox> comboBoxesesChildrenExsist = new List<ComboBox>();
        private List<Client> clients;
        private void addChildrenButton_Click(object sender, EventArgs e)
        {
            checkBoxAdultCard.Checked = false;
            tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.Controls.Add(new Label() { Text = "ФИО Ребенка", Anchor = AnchorStyles.None }, 0, tableLayoutPanel1.RowCount);
            richTextBoxesFIOchildrenNew.Add(new RichTextBox() { Name = "richTextBoxFIOchildren" + (tableLayoutPanel1.RowCount).ToString(), Size = new System.Drawing.Size(408, 54) });
            tableLayoutPanel1.Controls.Add(richTextBoxesFIOchildrenNew.Last(), 1, tableLayoutPanel1.RowCount);
            Button newButton = new Button();
            newButton.Text = "Удалить";
            Graphics cg = this.CreateGraphics();
            newButton.Font = new System.Drawing.Font("Arial Black", 14F);
            SizeF size = cg.MeasureString(newButton.Text, newButton.Font);
            newButton.Font = new System.Drawing.Font("Arial Black", 12F);
            newButton.Width = (int)size.Width;
            newButton.Height = (int)size.Height;
            newButton.Name = "deleteButton" + (tableLayoutPanel1.RowCount).ToString();
            newButton.Click += deleteButtonNew_Click;
            buttonsDeleteNew.Add(newButton);
            tableLayoutPanel1.Controls.Add(buttonsDeleteNew.Last(), 2, tableLayoutPanel1.RowCount);
            tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.Controls.Add(new Label() { Text = "Дата рождения ребенка", Anchor = AnchorStyles.Top }, 0, tableLayoutPanel1.RowCount);
            maskedTextBoxesChildrenNew.Add(new MaskedTextBox() { Name = "maskedTextBoxChildren" + (tableLayoutPanel1.RowCount).ToString(), Mask = "00/00/0000" });
            tableLayoutPanel1.Controls.Add(maskedTextBoxesChildrenNew.Last(), 1, tableLayoutPanel1.RowCount);
            tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.Controls.Add(new Label() { Text = "Пол", Anchor = AnchorStyles.Top }, 0, tableLayoutPanel1.RowCount);
            comboBoxesesChildrenNew.Add(new ComboBox() { Name = "comboBoxChildren" + (tableLayoutPanel1.RowCount).ToString() });
            comboBoxesesChildrenNew.Last().Items.Insert(0, "не выбран");
            comboBoxesesChildrenNew.Last().Items.Insert(1, "женский");
            comboBoxesesChildrenNew.Last().Items.Insert(2, "мужской");
            comboBoxesesChildrenNew.Last().SelectedIndex = 0;
            tableLayoutPanel1.Controls.Add(comboBoxesesChildrenNew.Last(), 1, tableLayoutPanel1.RowCount);
        }
        private void formLoad(int cardId, string loginCard, string ip)
        {
            ClientsInfo clientsInfoResponce = new ClientsInfo();
            clientsInfoResponce = GetClientsInfo(cardId, loginCard, ip);
            richTextBoxFIOparent.Text = clientsInfoResponce.parentName.ToString();
            clients = clientsInfoResponce.clients;
            comboBoxParentGender.Items.Insert(0, "не выбран");
            comboBoxParentGender.Items.Insert(1, "женский");
            comboBoxParentGender.Items.Insert(2, "мужской");
            comboBoxParentGender.SelectedIndex = clientsInfoResponce.clientContact.gender;
            maskedTextBoxParentBDay.Text = clientsInfoResponce.clientContact.bDay.ToShortDateString();
            maskedTextBoxTelephone.Text = clientsInfoResponce.clientContact.telephone;
            textBoxEmail.Text = clientsInfoResponce.clientContact.email;
            foreach (Client client in clients)
            {
                if ((bool)client.adultCard != true)
                {
                    checkBoxAdultCard.Checked = false;
                    tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "ФИО Ребенка", Anchor = AnchorStyles.None }, 0, tableLayoutPanel1.RowCount);
                    richTextBoxesFIOchildrenExsist.Add(new RichTextBox() { Name = "richTextBoxFIOchildren" + (tableLayoutPanel1.RowCount).ToString(), Text = client.childrenName.ToString(), Size = new System.Drawing.Size(408, 54) });
                    tableLayoutPanel1.Controls.Add(richTextBoxesFIOchildrenExsist.Last(), 1, tableLayoutPanel1.RowCount);
                    Button newButton = new Button();
                    newButton.Text = "Удалить";
                    Graphics cg = this.CreateGraphics();
                    newButton.Font = new System.Drawing.Font("Arial Black", 14F);
                    SizeF size = cg.MeasureString(newButton.Text, newButton.Font);
                    newButton.Width = (int)size.Width;
                    newButton.Height = (int)size.Height;
                    newButton.Name = "deleteButton" + (tableLayoutPanel1.RowCount).ToString();
                    newButton.Click += deleteButtonExsist_Click;
                    buttonsDeleteExist.Add(newButton);
                    tableLayoutPanel1.Controls.Add(buttonsDeleteExist.Last(), 2, tableLayoutPanel1.RowCount);
                    tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Дата рождения ребенка", Anchor = AnchorStyles.Top }, 0, tableLayoutPanel1.RowCount);
                    maskedTextBoxesChildrenExsist.Add(new MaskedTextBox() { Name = "maskedTextBoxChildren" + (tableLayoutPanel1.RowCount).ToString(), Text = ((DateTime)client.childrenDate).ToShortDateString(), Mask = "00/00/0000" });
                    tableLayoutPanel1.Controls.Add(maskedTextBoxesChildrenExsist.Last(), 1, tableLayoutPanel1.RowCount);
                    tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Пол", Anchor = AnchorStyles.Top }, 0, tableLayoutPanel1.RowCount);
                    comboBoxesesChildrenExsist.Add(new ComboBox() { Name = "comboBoxChildren" + (tableLayoutPanel1.RowCount).ToString() });
                    comboBoxesesChildrenExsist.Last().Items.Insert(0, "не выбран");
                    comboBoxesesChildrenExsist.Last().Items.Insert(1, "женский");
                    comboBoxesesChildrenExsist.Last().Items.Insert(2, "мужской");
                    comboBoxesesChildrenExsist.Last().SelectedIndex = client.gender;
                    tableLayoutPanel1.Controls.Add(comboBoxesesChildrenExsist.Last(), 1, tableLayoutPanel1.RowCount);
                }
                else
                {
                    checkBoxAdultCard.Checked = true;
                }
            }
            if (checkBoxAdultCard.Checked == true)
            {
                addChildrenButton.Enabled = false;
            }
            else
            {
                addChildrenButton.Enabled = true;
            }
        }
        private void deleteButtonExsist_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int indexOfDelete = buttonsDeleteExist.FindIndex(x => x.Name == btn.Name);
            if (richTextBoxesFIOchildrenExsist.ElementAt(indexOfDelete).Enabled == true && maskedTextBoxesChildrenExsist.ElementAt(indexOfDelete).Enabled == true)
            {
                richTextBoxesFIOchildrenExsist.ElementAt(indexOfDelete).Enabled = false;
                maskedTextBoxesChildrenExsist.ElementAt(indexOfDelete).Enabled = false;
                comboBoxesesChildrenExsist.ElementAt(indexOfDelete).Enabled = false;
                buttonsDeleteExist.ElementAt(indexOfDelete).Text = "Отменить удаление";
            }
            else
            {
                richTextBoxesFIOchildrenExsist.ElementAt(indexOfDelete).Enabled = true;
                maskedTextBoxesChildrenExsist.ElementAt(indexOfDelete).Enabled = true;
                comboBoxesesChildrenExsist.ElementAt(indexOfDelete).Enabled = true;
                buttonsDeleteExist.ElementAt(indexOfDelete).Text = "Удалить";
            }
        }
        private void deleteButtonNew_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int indexOfDelete = buttonsDeleteNew.FindIndex(x => x.Name == btn.Name);
            if (richTextBoxesFIOchildrenNew.ElementAt(indexOfDelete).Enabled == true && maskedTextBoxesChildrenNew.ElementAt(indexOfDelete).Enabled == true)
            {
                richTextBoxesFIOchildrenNew.ElementAt(indexOfDelete).Enabled = false;
                maskedTextBoxesChildrenNew.ElementAt(indexOfDelete).Enabled = false;
                comboBoxesesChildrenNew.ElementAt(indexOfDelete).Enabled = false;
                buttonsDeleteNew.ElementAt(indexOfDelete).Text = "Отменить удаление";
            }
            else
            {
                richTextBoxesFIOchildrenNew.ElementAt(indexOfDelete).Enabled = true;
                maskedTextBoxesChildrenNew.ElementAt(indexOfDelete).Enabled = true;
                comboBoxesesChildrenNew.ElementAt(indexOfDelete).Enabled = true;
                buttonsDeleteNew.ElementAt(indexOfDelete).Text = "Удалить";
            }
        }
        private void changeInfoButton_Click(object sender, EventArgs e)
        {
            try
            {
                string parentName = "";
                Card card = form.selectCardByNymber(cardId, loginCard, ip);
                ClientsInfo clientsInfo = new ClientsInfo();
                clientsInfo.clientContact = new ClientContact();
                int numberOfClients = 0;
                if (!(String.IsNullOrEmpty(richTextBoxFIOparent.Text.ToString())) && richTextBoxFIOparent.Text.ToString().Trim() != string.Empty)
                {
                    parentName = richTextBoxFIOparent.Text.ToString();
                }
                else
                {
                    throw new Exception("Введено не верное значение ФИО родителя");
                }
                if (!(String.IsNullOrEmpty(maskedTextBoxParentBDay.Text.ToString())) && (maskedTextBoxParentBDay.Text.ToString().Trim() != string.Empty) && (Regex.IsMatch(maskedTextBoxParentBDay.Text.ToString(), "^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)")))
                {
                    clientsInfo.clientContact.bDay = DateTime.Parse((maskedTextBoxParentBDay.Text));
                }
                else
                {
                    throw new Exception("Не правильно введена дата рождение родителя");
                }
                if (comboBoxParentGender.SelectedIndex >= 0 && comboBoxParentGender.SelectedIndex <= 2)
                {
                    clientsInfo.clientContact.gender = comboBoxParentGender.SelectedIndex;
                }
                if (!String.IsNullOrEmpty(textBoxEmail.Text.ToString()) && (textBoxEmail.Text.ToString().Trim() != string.Empty)
                    && (Regex.IsMatch(textBoxEmail.Text.ToString(), @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)")))
                {
                    clientsInfo.clientContact.email = textBoxEmail.Text;
                }
                else
                {
                    throw new Exception("Не правильно введен Email");
                }
                if (!String.IsNullOrEmpty(maskedTextBoxTelephone.Text.ToString()) && (maskedTextBoxTelephone.Text.ToString().Trim() != string.Empty)
                    && (Regex.IsMatch(maskedTextBoxTelephone.Text.ToString(), @"(\+7|8|\b)[\(\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[)\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)")))
                {
                    clientsInfo.clientContact.telephone = maskedTextBoxTelephone.Text;
                }
                else
                {
                    throw new Exception("Не правильно введен телефон");
                }
                if (!checkBoxAdultCard.Checked)
                {
                    clients.Clear();
                    for (int i = 7; i < tableLayoutPanel1.RowCount; i += 3)
                    {
                        bool enabledControl = true;
                        string childrenName = "";
                        string childrenDate = "";
                        int gender = 0;
                        foreach (Control control in this.tableLayoutPanel1.Controls)
                        {
                            TableLayoutPanelCellPosition controlPosition = new TableLayoutPanelCellPosition(1, i);
                            if (control is RichTextBox && tableLayoutPanel1.GetPositionFromControl(control) == controlPosition)
                            {
                                if (control.Enabled == true)
                                {
                                    enabledControl = true;
                                    if (!(String.IsNullOrEmpty(control.Text.ToString())) && control.Text.ToString().Trim() != string.Empty)
                                    {
                                        childrenName = control.Text.ToString();
                                    }
                                    else
                                    {
                                        throw new Exception("Введено не верное значение ФИО ребенка");
                                    }
                                }
                                else
                                {
                                    enabledControl = false;
                                }
                            }
                            controlPosition = new TableLayoutPanelCellPosition(1, i + 1);
                            if (control is MaskedTextBox && tableLayoutPanel1.GetPositionFromControl(control) == controlPosition)
                            {
                                if (control.Enabled == true)
                                {
                                    if (!(String.IsNullOrEmpty(control.Text.ToString())) && (control.Text.ToString().Trim() != string.Empty) && (Regex.IsMatch(control.Text.ToString(), "^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)")))
                                    {
                                        childrenDate = control.Text.ToString();
                                    }
                                    else
                                    {
                                        throw new Exception("Введено не верное значение даты рождения ребенка");
                                    }
                                }
                                else
                                {
                                    enabledControl = false;
                                }
                            }
                            controlPosition = new TableLayoutPanelCellPosition(1, i + 2);
                            if (control is ComboBox && tableLayoutPanel1.GetPositionFromControl(control) == controlPosition)
                            {
                                if (control.Enabled == true)
                                {
                                    if (((ComboBox)control).SelectedIndex >= 0 && (((ComboBox)control).SelectedIndex <= 2))
                                    {
                                        gender = (((ComboBox)control).SelectedIndex);
                                    }
                                    else
                                    {
                                        throw new Exception("Введено не верное значение даты рождения ребенка");
                                    }
                                }
                                else
                                {
                                    enabledControl = false;
                                }
                            }
                        }
                        if (enabledControl == true)
                        {
                            numberOfClients++;
                            clients.Add(new Client { cardId = card.cardId, childrenName = childrenName, childrenDate = DateTime.Parse(childrenDate), parentName = parentName, adultCard = false, gender = gender });
                        }

                    }


                }
                else if (checkBoxAdultCard.Checked && richTextBoxesFIOchildrenExsist.Count != 0)
                {
                    string message = "Данные о детях будут удалены. Продолжить?";
                    string caption = "Данные о детях";
                    //MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = new DialogResult();

                    FormMessage formMessage = new FormMessage(message, caption, result);
                    formMessage.ShowDialog();

                    if (formMessage.result == DialogResult.Yes)
                    {
                        clients.Clear();
                        numberOfClients = 0;
                        clients.Add(new Client { cardId = card.cardId, childrenName = "", childrenDate = new DateTime().AddYears(1900).AddMonths(1).AddDays(1), parentName = parentName, adultCard = true });
                    }
                    else
                    {
                        throw new Exception("Данные не внесенны");
                    }
                }
                else
                {
                    clients.Clear();
                    numberOfClients = 0;
                    clients.Add(new Client { cardId = card.cardId, childrenName = "", childrenDate = new DateTime().AddYears(1900).AddMonths(1).AddDays(1), parentName = parentName, adultCard = true });
                    

                }
                if (clients.Count != 0)
                {
                    clientsInfo = new ClientsInfo(cardId.ToString(), loginCard, ip, parentName, numberOfClients, clients, clientsInfo.clientContact);
                }
                else
                {
                    clients.Add(new Client { cardId = card.cardId, childrenName = "", childrenDate = new DateTime().AddYears(1900).AddMonths(1).AddDays(1), parentName = parentName, adultCard = true });
                    clientsInfo = new ClientsInfo(cardId.ToString(), loginCard, ip, parentName, numberOfClients, clients, clientsInfo.clientContact);
                }
                if (clientsInfo.clients.Count > 0)
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(APP_PATH + "/api/cashiermashine/ChangeClientInfoByCardNumber/");

                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = JsonConvert.SerializeObject(clientsInfo);
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    if (httpResponse.StatusCode == HttpStatusCode.Accepted)
                    {
                        if (tableLayoutPanel1.RowCount <= 6)
                        {
                            throw new Exception("Данные не добавленны.\nПроверте данные формы.");
                        }
                        else
                        {
                            FormMessage formMessage = new FormMessage("Данные успешно добавленны", "Касса");
                            formMessage.ShowDialog();
                            Close();
                        }
                    }
                    else if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                    {
                        throw new Exception("Ошибка при добавлении информации о клиентах");
                    }
                    else
                    {
                        throw new Exception(httpResponse.StatusDescription);
                    }
                }
            }
            catch (Exception exc)
            {
                FormMessage formMessage = new FormMessage(exc.Message, "Касса");
                formMessage.Show();
            }
        }
        private void checkBoxAdultCard_CheckedChanged(object sender, EventArgs e)
        {
            if (addChildrenButton.Enabled == false)
            {
                if ((richTextBoxesFIOchildrenNew.Count == 0 || maskedTextBoxesChildrenNew.Count == 0 || buttonsDeleteNew.Count == 0)
                && (richTextBoxesFIOchildrenExsist.Count == 0 || maskedTextBoxesChildrenExsist.Count == 0 || buttonsDeleteExist.Count == 0))
                {
                    tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "ФИО Ребенка", Anchor = AnchorStyles.None }, 0, tableLayoutPanel1.RowCount);
                    richTextBoxesFIOchildrenNew.Add(new RichTextBox() { Name = "richTextBoxFIOchildren" + (tableLayoutPanel1.RowCount).ToString(), Size = new System.Drawing.Size(408, 54) });
                    tableLayoutPanel1.Controls.Add(richTextBoxesFIOchildrenNew.Last(), 1, tableLayoutPanel1.RowCount);
                    Button newButton = new Button();
                    newButton.Text = "Удалить";
                    Graphics cg = this.CreateGraphics();
                    newButton.Font = new System.Drawing.Font("Arial Black", 14F);
                    SizeF size = cg.MeasureString(newButton.Text, newButton.Font);
                    newButton.Width = (int)size.Width;
                    newButton.Height = (int)size.Height;
                    newButton.Name = "deleteButton" + (tableLayoutPanel1.RowCount - 1).ToString();
                    newButton.Click += deleteButtonNew_Click;
                    buttonsDeleteNew.Add(newButton);
                    tableLayoutPanel1.Controls.Add(buttonsDeleteNew.Last(), 2, tableLayoutPanel1.RowCount);
                    tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Дата рождения ребенка", Anchor = AnchorStyles.Top }, 0, tableLayoutPanel1.RowCount);
                    maskedTextBoxesChildrenNew.Add(new MaskedTextBox() { Name = "maskedTextBoxChildren" + (tableLayoutPanel1.RowCount).ToString(), Mask = "00/00/0000" });
                    tableLayoutPanel1.Controls.Add(maskedTextBoxesChildrenNew.Last(), 1, tableLayoutPanel1.RowCount);
                    tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "Пол", Anchor = AnchorStyles.Top }, 0, tableLayoutPanel1.RowCount);
                    comboBoxesesChildrenNew.Add(new ComboBox() { Name = "comboBoxChildren" + (tableLayoutPanel1.RowCount).ToString() });
                    comboBoxesesChildrenNew.Last().Items.Insert(0, "не выбран");
                    comboBoxesesChildrenNew.Last().Items.Insert(1, "женский");
                    comboBoxesesChildrenNew.Last().Items.Insert(2, "мужской");
                    comboBoxesesChildrenNew.Last().SelectedIndex = 0;
                    tableLayoutPanel1.Controls.Add(comboBoxesesChildrenNew.Last(), 1, tableLayoutPanel1.RowCount);
                }
                addChildrenButton.Enabled = true;

            }
            else
            {
                addChildrenButton.Enabled = false;
            }
        }
        public ClientsInfo GetClientsInfo(int cardId, string loginCard, string ip)
        {
            APP_PATH = ConfigurationManager.AppSettings.Get("serverURI");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(APP_PATH + "/api/cashiermashine/GetClientsInfoByCardNumber/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            CardInfo cardInfo = new CardInfo(cardId.ToString(), loginCard, ip);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(cardInfo);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            ClientsInfo clientsInfo = new ClientsInfo();
            string result = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
                if (result == "null")
                {
                    return null;
                }
                else
                {
                    string r = result.Remove(result.Length - 1);
                    var k = JsonConvert.DeserializeObject(result);
                    clientsInfo = JsonConvert.DeserializeObject<ClientsInfo>(k.ToString());
                    return clientsInfo;
                }
            }
        }
        private void loadInfo()
        {

        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
