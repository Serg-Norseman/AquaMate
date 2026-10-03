/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

#pragma warning disable CA1416 // Validate platform compatibility

using System;
using System.Drawing;
using System.Windows.Forms;
using AquaMate.Core;
using AquaMate.UI;

namespace AquaMate.MCP;

public class MCPServerForm : Form
{
    private WFAppHost fInstance;

    private TextBox _hostTextBox;
    private TextBox _portTextBox;
    private TextBox _allowedHostsTextBox;
    private CheckBox _autoStartCheckBox;
    private CheckBox _corsCheckBox;
    private CheckBox _verboseLoggingCheckBox;
    private Button _startButton;
    private Button _stopButton;
    private Label _statusLabel;
    private GroupBox _groupBox;
    private TableLayoutPanel _mainLayoutPanel;
    private TableLayoutPanel _configLayoutPanel;
    private TableLayoutPanel _buttonsLayoutPanel;

    public MCPServerForm()
    {
        fInstance = (WFAppHost)AppHost.Instance;

        InitializeComponent();
        UpdateUIState();

        if (fInstance.IsRunning()) {
            _statusLabel.Text = string.Format(Localizer.LS(LSID.MCPServerStarted), $"http://{fInstance.ServerHost}:{fInstance.ServerPort}/mcp");
            _statusLabel.ForeColor = Color.Green;
        }
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        // Form properties
        this.Text = Localizer.LS(LSID.MCPMCPServerSettings);
        this.ClientSize = new Size(480, 320);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Create controls
        CreateControls();

        // Layout controls
        LayoutControls();

        // Resume layout
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private void CreateControls()
    {
        // Host label and textbox
        var lblHost = new Label();
        lblHost.Text = Localizer.LS(LSID.MCPServerHost);
        lblHost.AutoSize = true;
        lblHost.TextAlign = ContentAlignment.MiddleLeft;

        _hostTextBox = new TextBox();
        _hostTextBox.Text = fInstance.ServerHost;
        _hostTextBox.Width = 200;
        //_hostTextBox.ToolTipText = Localizer.LS(LSID.MCPHostToolTip);

        // Port label and textbox
        var lblPort = new Label();
        lblPort.Text = Localizer.LS(LSID.MCPServerPort);
        lblPort.AutoSize = true;
        lblPort.TextAlign = ContentAlignment.MiddleLeft;

        _portTextBox = new TextBox();
        _portTextBox.Text = fInstance.ServerPort.ToString();
        _portTextBox.Width = 200;
        //_portTextBox.ToolTipText = Localizer.LS(LSID.MCPPortToolTip);

        // CORS checkbox
        _corsCheckBox = new CheckBox();
        _corsCheckBox.Text = Localizer.LS(LSID.MCPCORS);
        _corsCheckBox.Checked = fInstance.EnableCors;
        _corsCheckBox.CheckedChanged += (sender, e) => {
            _allowedHostsTextBox.Enabled = _corsCheckBox.Checked;
        };
        _corsCheckBox.CheckedChanged += (s, e) => _allowedHostsTextBox.Enabled = _corsCheckBox.Checked;

        // Allowed hosts label and textbox
        var lblAllowedHosts = new Label();
        lblAllowedHosts.Text = Localizer.LS(LSID.MCPTrustedHosts);
        lblAllowedHosts.AutoSize = true;
        lblAllowedHosts.TextAlign = ContentAlignment.MiddleLeft;

        _allowedHostsTextBox = new TextBox();
        _allowedHostsTextBox.Text = fInstance.AllowedHosts;
        _allowedHostsTextBox.Width = 200;
        _allowedHostsTextBox.Enabled = _corsCheckBox.Checked;
        //_allowedHostsTextBox.ToolTipText = Localizer.LS(LSID.MCPAllowedHostsTip);

        // Verbose logging checkbox
        _verboseLoggingCheckBox = new CheckBox();
        _verboseLoggingCheckBox.Text = Localizer.LS(LSID.MCPVerboseServerLogs);
        _verboseLoggingCheckBox.Checked = fInstance.VerboseLogging;

        // Auto-start checkbox
        _autoStartCheckBox = new CheckBox();
        _autoStartCheckBox.Text = Localizer.LS(LSID.MCPAutoStart);
        _autoStartCheckBox.Checked = fInstance.AutoStart;
        _autoStartCheckBox.CheckedChanged += (s, e) => { fInstance.AutoStart = _autoStartCheckBox.Checked; };

        // Start button
        _startButton = new Button();
        _startButton.Text = Localizer.LS(LSID.MCPStart);
        _startButton.Size = new Size(80, 30);
        _startButton.UseVisualStyleBackColor = true;
        _startButton.Click += OnStartServerClick;

        // Stop button
        _stopButton = new Button();
        _stopButton.Text = Localizer.LS(LSID.MCPStop);
        _stopButton.Size = new Size(80, 30);
        _stopButton.UseVisualStyleBackColor = true;
        _stopButton.Click += OnStopServerClick;

        // Status label
        _statusLabel = new Label();
        _statusLabel.Text = Localizer.LS(LSID.MCPServerStopped);
        _statusLabel.AutoSize = true;
        _statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        _statusLabel.BorderStyle = BorderStyle.FixedSingle;
        _statusLabel.Padding = new Padding(5);
        _statusLabel.Dock = DockStyle.Fill;
    }

    private void LayoutControls()
    {
        // Create main layout panel
        _mainLayoutPanel = new TableLayoutPanel();
        _mainLayoutPanel.ColumnCount = 1;
        _mainLayoutPanel.RowCount = 3;
        _mainLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        _mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
        _mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
        _mainLayoutPanel.Dock = DockStyle.Fill;
        _mainLayoutPanel.Padding = new Padding(12);

        // Create config group box
        _groupBox = new GroupBox();
        _groupBox.Text = Localizer.LS(LSID.MCPMCPSrvConfig);
        _groupBox.Dock = DockStyle.Fill;

        // Create config layout panel
        _configLayoutPanel = new TableLayoutPanel();
        _configLayoutPanel.ColumnCount = 2;
        _configLayoutPanel.RowCount = 6;
        _configLayoutPanel.Dock = DockStyle.Fill;
        _configLayoutPanel.Padding = new Padding(10);
        _configLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        _configLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        // Add controls to config layout
        _configLayoutPanel.Controls.Add(new Label() { Text = Localizer.LS(LSID.MCPServerHost), TextAlign = ContentAlignment.MiddleLeft, AutoSize = true }, 0, 0);
        _configLayoutPanel.Controls.Add(_hostTextBox, 1, 0);

        _configLayoutPanel.Controls.Add(new Label() { Text = Localizer.LS(LSID.MCPServerPort), TextAlign = ContentAlignment.MiddleLeft, AutoSize = true }, 0, 1);
        _configLayoutPanel.Controls.Add(_portTextBox, 1, 1);

        // Empty cell for spacing
        _configLayoutPanel.Controls.Add(new Panel(), 0, 2);
        _configLayoutPanel.Controls.Add(_corsCheckBox, 1, 2);

        _configLayoutPanel.Controls.Add(new Label() { Text = Localizer.LS(LSID.MCPTrustedHosts), TextAlign = ContentAlignment.MiddleLeft, AutoSize = true }, 0, 3);
        _configLayoutPanel.Controls.Add(_allowedHostsTextBox, 1, 3);

        // Empty cell for spacing
        _configLayoutPanel.Controls.Add(new Panel(), 0, 4);
        _configLayoutPanel.Controls.Add(_verboseLoggingCheckBox, 1, 4);

        // Empty cell for spacing
        _configLayoutPanel.Controls.Add(new Panel(), 0, 5);
        _configLayoutPanel.Controls.Add(_autoStartCheckBox, 1, 5);

        _groupBox.Controls.Add(_configLayoutPanel);

        // Create buttons layout panel
        _buttonsLayoutPanel = new TableLayoutPanel();
        _buttonsLayoutPanel.ColumnCount = 3;
        _buttonsLayoutPanel.RowCount = 1;
        _buttonsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
        _buttonsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
        _buttonsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _buttonsLayoutPanel.Controls.Add(_startButton, 0, 0);
        _buttonsLayoutPanel.Controls.Add(_stopButton, 1, 0);
        _buttonsLayoutPanel.Controls.Add(_statusLabel, 2, 0);
        _buttonsLayoutPanel.Dock = DockStyle.Fill;

        // Add panels to main layout
        _mainLayoutPanel.Controls.Add(_groupBox, 0, 0);
        _mainLayoutPanel.Controls.Add(_buttonsLayoutPanel, 0, 1);

        // Add main layout to form
        this.Controls.Add(_mainLayoutPanel);
    }

    private async void OnStartServerClick(object sender, EventArgs e)
    {
        // Validate port
        if (!int.TryParse(_portTextBox.Text, out int port) || port < 1 || port > 65535) {
            MessageBox.Show(this, Localizer.LS(LSID.MCPValidPortRequired), Localizer.LS(LSID.MCPValidationError), MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        fInstance.ServerPort = port;

        UpdateUIState();
        _statusLabel.Text = Localizer.LS(LSID.MCPStartingServer);
        _statusLabel.ForeColor = Color.Orange;

        try {
            fInstance.ServerHost = _hostTextBox.Text;
            fInstance.EnableCors = _corsCheckBox.Checked;
            fInstance.AllowedHosts = _allowedHostsTextBox.Text;
            fInstance.VerboseLogging = _verboseLoggingCheckBox.Checked;

            await fInstance.StartAsync();

            _statusLabel.Text = string.Format(Localizer.LS(LSID.MCPServerStarted), $"http://{fInstance.ServerHost}:{fInstance.ServerPort}/mcp");
            _statusLabel.ForeColor = Color.Green;
        } catch (Exception ex) {
            _statusLabel.Text = Localizer.LS(LSID.MCPErrorStartingServer);
            _statusLabel.ForeColor = Color.Red;
            MessageBox.Show(this, string.Format(Localizer.LS(LSID.MCPStartingError), ex.Message), Localizer.LS(LSID.MCPError), MessageBoxButtons.OK, MessageBoxIcon.Error);
        } finally {
            UpdateUIState();
        }
    }

    private async void OnStopServerClick(object sender, EventArgs e)
    {
        _statusLabel.Text = Localizer.LS(LSID.MCPStoppingServer);
        _statusLabel.ForeColor = Color.Orange;
        _stopButton.Enabled = false;

        try {
            await fInstance.StopAsync();

            _statusLabel.Text = Localizer.LS(LSID.MCPServerStopped);
            _statusLabel.ForeColor = SystemColors.ControlText;
        } catch (Exception ex) {
            MessageBox.Show(this, string.Format(Localizer.LS(LSID.MCPStoppingError), ex.Message), Localizer.LS(LSID.MCPError), MessageBoxButtons.OK, MessageBoxIcon.Error);
        } finally {
            UpdateUIState();
        }
    }

    private void UpdateUIState()
    {
        var isRunning = fInstance.IsRunning();

        _startButton.Enabled = !isRunning;
        _stopButton.Enabled = isRunning;

        _hostTextBox.Enabled = !isRunning;
        _portTextBox.Enabled = !isRunning;
        _allowedHostsTextBox.Enabled = !isRunning && _corsCheckBox.Checked;
        _corsCheckBox.Enabled = !isRunning;
        _verboseLoggingCheckBox.Enabled = !isRunning;
        _autoStartCheckBox.Enabled = !isRunning;
    }
}
