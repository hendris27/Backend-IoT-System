Imports Ivi.DCPwr.Interop
Imports Microsoft.VisualBasic
Imports System
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Math
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Point
Imports System.Diagnostics
Imports Tektronix.TekPWS4000.Interop
Imports MySql.Data.MySqlClient

Public Delegate Sub FormDelegate()

Public Class Main
    Inherits System.Windows.Forms.Form

#Region "Variable Power Supply Tektronix"
    Dim myPWS As New TekPWS4000
    Dim myPWSOutPut As TekPWS4000OutputChannel
    Dim myVoltage As Double
    Dim myCurrent As Double
#End Region

#Region "Variable DIO 48"
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)
    Private PortType As Integer
    Private PortNum As MccDaq.DigitalPortType
    Private NumPorts, NumBits, MaxPortVal As Integer
    Private ProgAbility As Integer
    Private Direction As MccDaq.DigitalPortDirection
    Dim FirstBit As Integer
    Dim ULStat As MccDaq.ErrorInfo
    Dim DataValue As UInt16
    Dim Relay As Short
    Dim R1, R2, R3, R4, R5, R6, R7, R8, R9, R10,
        R11, R12, R13, R14, R15, R16, R17, R18, R19, R20,
        R21, R22, R23, R24, R25, R26, R27, R28, R29, R30,
        R31, R32, R33, R34, R35, R36, R37, R38, R39, R40 As Byte
    Dim a, b, c, d As Byte
    Dim i, j, k, l As Integer
#End Region

#Region "Variable PCI 7296 I/O"
    Public Card As Integer
    Dim Result As Integer
    Dim RegCard As Boolean
    Dim ValPort1A As Long
    Dim ValPort1B As Long
    Dim ValPort1CL As Long
    Dim ValPort1CH As Long
    Dim ValPort2A As Long
    Dim ValPort2B As Long
    Dim ValPort2CL As Long
    Dim ValPort2CH As Long
    Dim ValPort3A As Long
    Dim ValPort3B As Long
    Dim ValPort3CL As Long
    Dim ValPort3CH As Long
    Dim ValPort4A As Long
    Dim ValPort4B As Long
    Dim ValPort4CL As Long
    Dim ValPort4CH As Long
#End Region

#Region "Variable CIM"
    Dim Barcode, Model As String
    Dim Content102, Condition As String
    Dim CIMRun As Boolean
    Dim File As Integer = FreeFile()
    Dim FileDir As String
    Dim FileReader As System.IO.StreamReader
    Dim LoopDir As Integer = 0
    Dim SelectModel As Boolean = False
    Dim NGType As String
#End Region

#Region "Variable Serial Nest 1"
    Dim inbufflenN1 As Long
    Dim cntlpN1 As Long
    Dim inbuffcntN1 As Long
    Dim tempstrN1 As String
    Dim strbuffN1 As String
    Dim SerialInN1, SerialOutN1 As String
    Dim minorRomN1 As Integer
    Dim majorRomN1 As Integer
    Public strhexbuffN1(1000) As String
    Dim tempbuffN1 As String
#End Region

#Region "Variable Serial Nest 2"
    Dim inbufflenN2 As Long
    Dim cntlpN2 As Long
    Dim inbuffcntN2 As Long
    Dim tempstrN2 As String
    Dim strbuffN2 As String
    Dim SerialInN2, SerialOutN2 As String
    Dim minorRomN2 As Integer
    Dim majorRomN2 As Integer
    Public strhexbuffN2(1000) As String
    Dim tempbuffN2 As String
#End Region

#Region "Variable Serial Nest 3"
    Dim inbufflenN3 As Long
    Dim cntlpN3 As Long
    Dim inbuffcntN3 As Long
    Dim tempstrN3 As String
    Dim strbuffN3 As String
    Dim SerialInN3, SerialOutN3 As String
    Dim minorRomN3 As Integer
    Dim majorRomN3 As Integer
    Public strhexbuffN3(1000) As String
    Dim tempbuffN3 As String
#End Region

#Region "Variable Serial Nest 4"
    Dim inbufflenN4 As Long
    Dim cntlpN4 As Long
    Dim inbuffcntN4 As Long
    Dim tempstrN4 As String
    Dim strbuffN4 As String
    Dim SerialInN4, SerialOutN4 As String
    Dim minorRomN4 As Integer
    Dim majorRomN4 As Integer
    Public strhexbuffN4(1000) As String
    Dim tempbuffN4 As String
#End Region

    'Dim str As String = "server=10.14.81.14; uid=general; pwd=Avi123!; database=recordsn"
    'Dim con As New MySqlConnection(str)

    Dim AutoMode As Boolean
    Dim CheckPoint As String
    Public ItemCheck(20) As String
    Public StepTest As Integer
    Dim Response As Integer
    Dim CounterOKN1, CounterNGN1, CounterOKN2, CounterNGN2, CounterOKN3, CounterNGN3, CounterOKN4, CounterNGN4 As Integer
    Public RunningNest1, RunningNest2, RunningNest3, RunningNest4 As Boolean
    Declare Function GetTickCount Lib "kernel32" () As Integer
    Dim ConditionNest1 As Boolean
    Dim ConditionNest2 As Boolean
    Dim ConditionNest3 As Boolean
    Dim ConditionNest4 As Boolean
    Dim BarcodeN1, BarcodeN2, BarcodeN3, BarcodeN4 As String
    Dim PushButtonEmergency As Boolean
    Dim PushButtonLeft As Boolean
    Dim PushButtonRight As Boolean
    Dim ButtonReady As Boolean = False
    Dim Condition1 As Boolean = False
    Dim Condition2 As Boolean = False
    Dim Condition3 As Boolean = False
    Dim Condition4 As Boolean = False
    Dim BarcodeStep1 As Boolean = False
    Dim BarcodeStep2 As Boolean = False
    Dim BarcodeStep3 As Boolean = False
    Public ProcessN1, ProcessN2, ProcessN3, ProcessN4 As Boolean
    Dim RecheckStepN1, RecheckStepN2, RecheckStepN3, RecheckStepN4 As Boolean

    Dim SelectModelN1 As Boolean = False
    Dim SelectModelN2 As Boolean = False
    Dim SelectModelN3 As Boolean = False
    Dim SelectModelN4 As Boolean = False

    Dim SafetySensor As Boolean = False

    Public ModelSelectN1 As Boolean = False
    Public ModelSelectN2 As Boolean = False
    Public ModelSelectN3 As Boolean = False
    Public ModelSelectN4 As Boolean = False

    Dim ModelN1, ModelN2, ModelN3, ModelN4 As String

    Public Model_SelectModelN1 As Boolean = False
    Public Model_SelectModelN2 As Boolean = False
    Public Model_SelectModelN3 As Boolean = False
    Public Model_SelectModelN4 As Boolean = False

    Dim Barcode_Ready_Auto As Boolean = False
    ' Nest 1/2 must complete both scans and release the start buttons before testing.
    Dim RequirePostBarcodeStart As Boolean = False
    Dim StartButtonReleasedAfterBarcode As Boolean = False

    Dim AutoReady As Boolean = False
    Dim AllProcess As Boolean
    Dim Interrupt As Boolean
    Dim StepProcess As Integer
    Private Stopwatch As New Stopwatch

    Dim CharROM(2) As String
    Dim CharNVM(3) As String
    Dim CharROMK2SG(2) As String
    Dim CharNVMK2SG(3) As String
    Dim CharROMK3MF(2) As String
    Dim CharNVMK3MF(3) As String
    Dim StepModify As Integer

    Dim Convert2Integer As String
    Dim Convert2Integer2 As String
    Dim Convert2Integer3 As String
    Dim Convert2Integer4 As String
    Dim Convert2Integer5 As String

    Public ValidateNVMN1 As Boolean = False
    Public ValidateNVMN2 As Boolean = False
    Public ValidateNVMN3 As Boolean = False
    Public ValidateNVMN4 As Boolean = False

    Dim CaseElseCondition As Boolean = False

    Dim BootmodeV, OperationV As Decimal

    Dim CountLoop As Integer = 0

    Dim TextAdd1, TextAdd2, TextAdd3, TextAdd4, TextAdd5, TextMaster, Day, Month, Year, Hour, Minute, Second, Print As String
    Dim TextAdd(6) As String
    Dim LenTextMaster, LenTextAdd, SelectFor As Integer
    Dim NGRateN1, NGRateN2, NGRateN3, NGRateN4 As Double

    Private Sub InitUL()
        ULStat = MccDaq.MccService.DeclareRevision(MccDaq.MccService.CurrentRevNum)
        ReportError = MccDaq.ErrorReporting.PrintAll
        HandleError = MccDaq.ErrorHandling.StopAll
        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If
    End Sub

    Private Sub SettingIO()
        ULStat = DaqBoard.DConfigPort(MccDaq.DigitalPortType.FirstPortA, MccDaq.DigitalPortDirection.DigitalOut)
        ULStat = DaqBoard.DConfigPort(MccDaq.DigitalPortType.FirstPortB, MccDaq.DigitalPortDirection.DigitalOut)
        ULStat = DaqBoard.DConfigPort(MccDaq.DigitalPortType.FirstPortCL, MccDaq.DigitalPortDirection.DigitalOut)
        ULStat = DaqBoard.DConfigPort(MccDaq.DigitalPortType.FirstPortCH, MccDaq.DigitalPortDirection.DigitalOut)
        ULStat = DaqBoard.DConfigPort(MccDaq.DigitalPortType.SecondPortB, MccDaq.DigitalPortDirection.DigitalOut)
        ULStat = DaqBoard.DConfigPort(MccDaq.DigitalPortType.SecondPortCL, MccDaq.DigitalPortDirection.DigitalOut)
        ULStat = DaqBoard.DConfigPort(MccDaq.DigitalPortType.SecondPortCH, MccDaq.DigitalPortDirection.DigitalOut)

        ULStat = DaqBoard.DConfigPort(MccDaq.DigitalPortType.SecondPortA, MccDaq.DigitalPortDirection.DigitalIn)
    End Sub

    Private Sub ResetIO()
        Output()
        ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.FirstPortA, 0)
        ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.FirstPortB, 0)
        ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.FirstPortCH, 0)
        ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.FirstPortCL, 0)
        ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.SecondPortA, 0)
        ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.SecondPortB, 0)
        ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.SecondPortCH, 0)
        ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.SecondPortCL, 0)
    End Sub

    Function RelayBoard(ByVal RelayNumb As Integer, ByVal Relaystate As Integer) As Integer
        If RelayNumb = 1 And Relaystate = 0 Then R1 = 0
        If RelayNumb = 1 And Relaystate = 1 Then R1 = 1
        If RelayNumb = 2 And Relaystate = 0 Then R2 = 0
        If RelayNumb = 2 And Relaystate = 1 Then R2 = 1
        If RelayNumb = 3 And Relaystate = 0 Then R3 = 0
        If RelayNumb = 3 And Relaystate = 1 Then R3 = 1
        If RelayNumb = 4 And Relaystate = 0 Then R4 = 0
        If RelayNumb = 4 And Relaystate = 1 Then R4 = 1
        If RelayNumb = 5 And Relaystate = 0 Then R5 = 0
        If RelayNumb = 5 And Relaystate = 1 Then R5 = 1
        If RelayNumb = 6 And Relaystate = 0 Then R6 = 0
        If RelayNumb = 6 And Relaystate = 1 Then R6 = 1
        If RelayNumb = 7 And Relaystate = 0 Then R7 = 0
        If RelayNumb = 7 And Relaystate = 1 Then R7 = 1
        If RelayNumb = 8 And Relaystate = 0 Then R8 = 0
        If RelayNumb = 8 And Relaystate = 1 Then R8 = 1
        If RelayNumb = 9 And Relaystate = 0 Then R9 = 0
        If RelayNumb = 9 And Relaystate = 1 Then R9 = 1
        If RelayNumb = 10 And Relaystate = 0 Then R10 = 0
        If RelayNumb = 10 And Relaystate = 1 Then R10 = 1
        If RelayNumb = 11 And Relaystate = 0 Then R11 = 0
        If RelayNumb = 11 And Relaystate = 1 Then R11 = 1
        If RelayNumb = 12 And Relaystate = 0 Then R12 = 0
        If RelayNumb = 12 And Relaystate = 1 Then R12 = 1
        If RelayNumb = 13 And Relaystate = 0 Then R13 = 0
        If RelayNumb = 13 And Relaystate = 1 Then R13 = 1
        If RelayNumb = 14 And Relaystate = 0 Then R14 = 0
        If RelayNumb = 14 And Relaystate = 1 Then R14 = 1
        If RelayNumb = 15 And Relaystate = 0 Then R15 = 0
        If RelayNumb = 15 And Relaystate = 1 Then R15 = 1
        If RelayNumb = 16 And Relaystate = 0 Then R16 = 0
        If RelayNumb = 16 And Relaystate = 1 Then R16 = 1
        If RelayNumb = 17 And Relaystate = 0 Then R17 = 0
        If RelayNumb = 17 And Relaystate = 1 Then R17 = 1
        If RelayNumb = 18 And Relaystate = 0 Then R18 = 0
        If RelayNumb = 18 And Relaystate = 1 Then R18 = 1
        If RelayNumb = 19 And Relaystate = 0 Then R19 = 0
        If RelayNumb = 19 And Relaystate = 1 Then R19 = 1
        If RelayNumb = 20 And Relaystate = 0 Then R20 = 0
        If RelayNumb = 20 And Relaystate = 1 Then R20 = 1
        If RelayNumb = 21 And Relaystate = 0 Then R21 = 0
        If RelayNumb = 21 And Relaystate = 1 Then R21 = 1
        If RelayNumb = 22 And Relaystate = 0 Then R22 = 0
        If RelayNumb = 22 And Relaystate = 1 Then R22 = 1
        If RelayNumb = 23 And Relaystate = 0 Then R23 = 0
        If RelayNumb = 23 And Relaystate = 1 Then R23 = 1
        If RelayNumb = 24 And Relaystate = 0 Then R24 = 0
        If RelayNumb = 24 And Relaystate = 1 Then R24 = 1
        If RelayNumb = 25 And Relaystate = 0 Then R25 = 0
        If RelayNumb = 25 And Relaystate = 1 Then R25 = 1
        If RelayNumb = 26 And Relaystate = 0 Then R26 = 0
        If RelayNumb = 26 And Relaystate = 1 Then R26 = 1
        If RelayNumb = 27 And Relaystate = 0 Then R27 = 0
        If RelayNumb = 27 And Relaystate = 1 Then R27 = 1
        If RelayNumb = 28 And Relaystate = 0 Then R28 = 0
        If RelayNumb = 28 And Relaystate = 1 Then R28 = 1
        If RelayNumb = 29 And Relaystate = 0 Then R29 = 0
        If RelayNumb = 29 And Relaystate = 1 Then R29 = 1
        If RelayNumb = 30 And Relaystate = 0 Then R30 = 0
        If RelayNumb = 30 And Relaystate = 1 Then R30 = 1
        If RelayNumb = 31 And Relaystate = 0 Then R31 = 0
        If RelayNumb = 31 And Relaystate = 1 Then R31 = 1
        If RelayNumb = 32 And Relaystate = 0 Then R32 = 0
        If RelayNumb = 32 And Relaystate = 1 Then R32 = 1
        If RelayNumb = 33 And Relaystate = 0 Then R33 = 0
        If RelayNumb = 33 And Relaystate = 1 Then R33 = 1
        If RelayNumb = 34 And Relaystate = 0 Then R34 = 0
        If RelayNumb = 34 And Relaystate = 1 Then R34 = 1
        If RelayNumb = 35 And Relaystate = 0 Then R35 = 0
        If RelayNumb = 35 And Relaystate = 1 Then R35 = 1
        If RelayNumb = 36 And Relaystate = 0 Then R36 = 0
        If RelayNumb = 36 And Relaystate = 1 Then R36 = 1
        If RelayNumb = 37 And Relaystate = 0 Then R37 = 0
        If RelayNumb = 37 And Relaystate = 1 Then R37 = 1
        If RelayNumb = 38 And Relaystate = 0 Then R38 = 0
        If RelayNumb = 38 And Relaystate = 1 Then R38 = 1
        If RelayNumb = 39 And Relaystate = 0 Then R39 = 0
        If RelayNumb = 39 And Relaystate = 1 Then R39 = 1
        If RelayNumb = 40 And Relaystate = 0 Then R40 = 0
        If RelayNumb = 40 And Relaystate = 1 Then R40 = 1

        'FirstPortA 8Relay (Relay ke 1-8)
        If RelayNumb = 1 Or RelayNumb = 2 Or RelayNumb = 3 Or RelayNumb = 4 Or RelayNumb = 5 Or RelayNumb = 6 Or RelayNumb = 7 Or RelayNumb = 8 Then
            Relay = R1 * 1 + R2 * 2 + R3 * 4 + R4 * 8 + R5 * 16 + R6 * 32 + R7 * 64 + R8 * 128
            ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.FirstPortA, Relay)
        End If

        'FirstPortB 8Relay (Relay ke 9-16)
        If RelayNumb = 9 Or RelayNumb = 10 Or RelayNumb = 11 Or RelayNumb = 12 Or RelayNumb = 13 Or RelayNumb = 14 Or RelayNumb = 15 Or RelayNumb = 16 Then
            Relay = R9 * 1 + R10 * 2 + R11 * 4 + R12 * 8 + R13 * 16 + R14 * 32 + R15 * 64 + R16 * 128
            ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.FirstPortB, Relay)
        End If

        'FirstPortCL 4Relay (Relay ke 17-20)
        If RelayNumb = 17 Or RelayNumb = 18 Or RelayNumb = 19 Or RelayNumb = 20 Then
            Relay = R17 * 1 + R18 * 2 + R19 * 4 + R20 * 8
            ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.FirstPortCL, Relay)
        End If

        'FirstPortCH 4Relay (Relay ke 21-24)
        If RelayNumb = 21 Or RelayNumb = 22 Or RelayNumb = 23 Or RelayNumb = 24 Then
            Relay = R21 * 1 + R22 * 2 + R23 * 4 + R24 * 8
            ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.FirstPortCH, Relay)
        End If
        'SecondPortB 8Relay (Relay ke 25-32)
        If RelayNumb = 25 Or RelayNumb = 26 Or RelayNumb = 27 Or RelayNumb = 28 Or RelayNumb = 29 Or RelayNumb = 30 Or RelayNumb = 31 Or RelayNumb = 32 Then
            Relay = R25 * 1 + R26 * 2 + R27 * 4 + R28 * 8 + R29 * 16 + R30 * 32 + R31 * 64 + R32 * 128
            ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.SecondPortB, Relay)
        End If

        'SecondPortCL 4Relay (Relay ke 33-36)
        If RelayNumb = 33 Or RelayNumb = 34 Or RelayNumb = 35 Or RelayNumb = 36 Then
            Relay = R33 * 1 + R34 * 2 + R35 * 4 + R36 * 8
            ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.SecondPortCL, Relay)
        End If
        'SecondPortCH 4Relay (Relay ke 37-40)
        If RelayNumb = 37 Or RelayNumb = 38 Or RelayNumb = 39 Or RelayNumb = 40 Then
            Relay = R37 * 1 + R38 * 2 + R39 * 4 + R40 * 8
            ULStat = DaqBoard.DOut(MccDaq.DigitalPortType.SecondPortCH, Relay)
        End If

        Return Relay
    End Function

    Sub Input()
        Direction = MccDaq.DigitalPortDirection.DigitalIn
        PortType = PORTIN
        NumPorts = FindPortsOfType(DaqBoard, PortType, ProgAbility, PortNum, NumBits, FirstBit)
    End Sub

    Sub Output()
        Direction = MccDaq.DigitalPortDirection.DigitalOut
        PortType = PORTOUT
        NumPorts = FindPortsOfType(DaqBoard, PortType, ProgAbility, PortNum, NumBits, FirstBit)
    End Sub

    Private Sub Initial_Card()
        Result = DIO_PortConfig(Card, Channel_P1A, OUTPUT_PORT)
        Result = DIO_PortConfig(Card, Channel_P1B, OUTPUT_PORT)
        Result = DIO_PortConfig(Card, Channel_P1CL, OUTPUT_PORT)
        Result = DIO_PortConfig(Card, Channel_P1CH, OUTPUT_PORT)

        Result = DIO_PortConfig(Card, Channel_P2A, OUTPUT_PORT)
        Result = DIO_PortConfig(Card, Channel_P2B, OUTPUT_PORT)
        Result = DIO_PortConfig(Card, Channel_P2CL, OUTPUT_PORT)
        Result = DIO_PortConfig(Card, Channel_P2CH, OUTPUT_PORT)

        Result = DIO_PortConfig(Card, Channel_P3A, INPUT_PORT)
        Result = DIO_PortConfig(Card, Channel_P3B, INPUT_PORT)
        Result = DIO_PortConfig(Card, Channel_P3CL, INPUT_PORT)
        Result = DIO_PortConfig(Card, Channel_P3CH, INPUT_PORT)

        Result = DIO_PortConfig(Card, Channel_P4A, OUTPUT_PORT)
        Result = DIO_PortConfig(Card, Channel_P4B, OUTPUT_PORT)
        Result = DIO_PortConfig(Card, Channel_P4CL, OUTPUT_PORT)
        Result = DIO_PortConfig(Card, Channel_P4CH, OUTPUT_PORT)
    End Sub

    Sub ClearPort()
        ValPort1A = 0
        ValPort1B = 0
        ValPort1CL = 0
        ValPort1CH = 0

        ValPort2A = 0
        ValPort2B = 0
        ValPort2CL = 0
        ValPort2CH = 0

        ValPort4A = 0
        ValPort4B = 0
        ValPort4CL = 0
        ValPort4CH = 0
        Wr_DoPort()
    End Sub

    Private Sub Wr_DoPort()
        If RegCard = True Then
            Result = DO_WritePort(Card, Channel_P1A, ValPort1A)
            Result = DO_WritePort(Card, Channel_P1B, ValPort1B)
            Result = DO_WritePort(Card, Channel_P1CL, ValPort1CL)
            Result = DO_WritePort(Card, Channel_P1CH, ValPort1CH)

            Result = DO_WritePort(Card, Channel_P2A, ValPort2A)
            Result = DO_WritePort(Card, Channel_P2B, ValPort2B)
            Result = DO_WritePort(Card, Channel_P2CL, ValPort2CL)
            Result = DO_WritePort(Card, Channel_P2CH, ValPort2CH)

            Result = DO_WritePort(Card, Channel_P4A, ValPort4A)
            Result = DO_WritePort(Card, Channel_P4B, ValPort4B)
            Result = DO_WritePort(Card, Channel_P4CL, ValPort4CL)
            Result = DO_WritePort(Card, Channel_P4CH, ValPort4CH)
        End If
    End Sub

    Private Sub Rd_DiPort()
        If RegCard = True Then
            Result = DI_ReadPort(Card, Channel_P3A, ValPort3A)
            Result = DI_ReadPort(Card, Channel_P3B, ValPort3B)
            Result = DI_ReadPort(Card, Channel_P3CL, ValPort3CL)
            Result = DI_ReadPort(Card, Channel_P3CH, ValPort3CH)
        End If
    End Sub

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBoxModel.Text = ""
        AutoMode = False

        Card = Register_Card(PCI_7296, 0)
        If Card < 0 Then
            MessageBox.Show("PCI 7296 (Nest 3 & Nest 4) Not Detected!", "FCT K59J-K2VG #2", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        If (Card >= 0) Then
            RegCard = True
            Initial_Card()
            ClearPort()
        Else
            RegCard = False
        End If

        InitUL()
        SettingIO()
        ResetIO()

        CounterOKN1 = 0
        CounterNGN1 = 0

        CounterOKN2 = 0
        CounterNGN2 = 0

        CounterOKN3 = 0
        CounterNGN3 = 0

        CounterOKN4 = 0
        CounterNGN4 = 0

        GroupBoxManual.Visible = False

        LabelCounterOKN1.Text = My.Settings.CounterOKN1
        LabelCounterNGN1.Text = My.Settings.CounterNGN1
        LabelCounterOKN2.Text = My.Settings.CounterOKN2
        LabelCounterNGN2.Text = My.Settings.CounterNGN2
        LabelCounterOKN3.Text = My.Settings.CounterOKN3
        LabelCounterNGN3.Text = My.Settings.CounterNGN3
        LabelCounterOKN4.Text = My.Settings.CounterOKN4
        LabelCounterNGN4.Text = My.Settings.CounterNGN4

        RunningNest1 = False
        RunningNest2 = False
        RunningNest3 = False
        RunningNest4 = False

        ProcessN1 = True
        ProcessN2 = True
        ProcessN3 = True
        ProcessN4 = True

        AllProcess = True
        Interrupt = True

        PushButtonEmergency = False
        PushButtonLeft = False
        PushButtonRight = False

        LabelInfoCIM.Text = ""

        CharROM(1) = "01"
        CharROM(2) = "06"
        CharNVM(1) = "01"
        CharNVM(2) = "06"
        CharNVM(3) = "01"

        BootmodeV = My.Settings.BootmodeV
        OperationV = My.Settings.OperationV

        TextBoxBootmodeV.Text = BootmodeV
        TextBoxOperationV.Text = OperationV

        If myPWS.Initialized = False Then
            myPWS.Initialize("USB0::0x0699::0x0390::C010255::0::INSTR", True, False)
            myPWSOutPut = myPWS.Outputs.Item(myPWS.Outputs.Name(1))
            myPWS.System.ControlMode = TekPWS4000ControlModeEnum.TekPWS4000ControlModeRemote
            myPWSOutPut.Enabled = False
            myPWSOutPut.VoltageLevel = BootmodeV
            myPWSOutPut.CurrentLimit = 5.0
            myPWSOutPut.Enabled = True
        End If

        DataGridViewProcessNest1.Rows.Add(40)
        DataGridViewProcessNest2.Rows.Add(40)
        DataGridViewProcessNest3.Rows.Add(40)
        DataGridViewProcessNest4.Rows.Add(40)

        ItemCheck(1) = "N/A"
        ItemCheck(2) = "D609 - Turn Left"
        ItemCheck(3) = "D603 - Smart"
        ItemCheck(4) = "D605 - High Beam"
        ItemCheck(5) = "D604 - Engine"
        ItemCheck(6) = "D607 - TW"
        ItemCheck(7) = "D611 - ISS"
        ItemCheck(8) = "D608 - Battery Fail"
        ItemCheck(9) = "D610 - Turn Right"
        ItemCheck(10) = "D606 - Alarm"

        For j As Integer = 0 To 39
            DataGridViewProcessNest1.Rows.Item(j).Cells(0).Value = j
            DataGridViewProcessNest2.Rows.Item(j).Cells(0).Value = j
            DataGridViewProcessNest3.Rows.Item(j).Cells(0).Value = j
            DataGridViewProcessNest4.Rows.Item(j).Cells(0).Value = j
        Next j

        Feasa.Show()
        Feasa.WindowState = Windows.Forms.FormWindowState.Minimized

    End Sub

    Private Sub ButtonResetCounterN1_Click(sender As Object, e As EventArgs) Handles ButtonResetCounterN1.Click
        'RecordDataN1()
        'NGRateN1 = 0
        CounterOKN1 = 0
        CounterNGN1 = 0
        My.Settings.CounterOKN1 = CounterOKN1
        My.Settings.CounterNGN1 = CounterNGN1
        My.Settings.Save()
        LabelCounterOKN1.Text = My.Settings.CounterOKN1
        LabelCounterNGN1.Text = My.Settings.CounterNGN1
    End Sub

    Private Sub ButtonResetCounterN2_Click(sender As Object, e As EventArgs) Handles ButtonResetCounterN2.Click
        'RecordDataN2()
        'NGRateN2 = 0
        CounterOKN2 = 0
        CounterNGN2 = 0
        My.Settings.CounterOKN2 = CounterOKN2
        My.Settings.CounterNGN2 = CounterNGN2
        My.Settings.Save()
        LabelCounterOKN2.Text = My.Settings.CounterOKN2
        LabelCounterNGN2.Text = My.Settings.CounterNGN2
    End Sub

    Private Sub ButtonResetCounterN3_Click(sender As Object, e As EventArgs) Handles ButtonResetCounterN3.Click
        'RecordDataN3()
        'NGRateN3 = 0
        CounterOKN3 = 0
        CounterNGN3 = 0
        My.Settings.CounterOKN3 = CounterOKN3
        My.Settings.CounterNGN3 = CounterNGN3
        My.Settings.Save()
        LabelCounterOKN3.Text = My.Settings.CounterOKN3
        LabelCounterNGN3.Text = My.Settings.CounterNGN3
    End Sub

    Private Sub ButtonResetCounterN4_Click(sender As Object, e As EventArgs) Handles ButtonResetCounterN4.Click
        'RecordDataN4()
        'NGRateN4 = 0
        CounterOKN4 = 0
        CounterNGN4 = 0
        My.Settings.CounterOKN4 = CounterOKN4
        My.Settings.CounterNGN4 = CounterNGN4
        My.Settings.Save()
        LabelCounterOKN4.Text = My.Settings.CounterOKN4
        LabelCounterNGN4.Text = My.Settings.CounterNGN4
    End Sub

    Private Sub AutoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutoToolStripMenuItem.Click
        MessageBox.Show("Auto Mode!", "FCT K59J-K2VG #2", MessageBoxButtons.OK, MessageBoxIcon.Information)
        AutoMode = True
        If AutoMode = True Then
            Timer2.Enabled = True

            TextBoxBarcode.Text = ""
            TextBoxBarcode.Focus()

            TextBoxBarcodeN1.Clear()
            TextBoxBarcodeN1.BackColor = Color.White
            TextBoxBarcodeN1.ReadOnly = False

            TextBoxBarcodeN2.Clear()
            TextBoxBarcodeN2.BackColor = Color.White
            TextBoxBarcodeN2.ReadOnly = False

            TextBoxBarcodeN3.Clear()
            TextBoxBarcodeN3.BackColor = Color.White
            TextBoxBarcodeN3.ReadOnly = False

            TextBoxBarcodeN4.Clear()
            TextBoxBarcodeN4.BackColor = Color.White
            TextBoxBarcodeN4.ReadOnly = False

            BarcodeN1 = ""
            BarcodeN2 = ""
            BarcodeN3 = ""
            BarcodeN4 = ""

            If Timer2.Enabled = True Then
                If CheckBoxSendCIM.Checked = True Then
                    LabelInfoCIM.Text = "SEND CIM DATA"
                    LabelInfoCIM.BackColor = Color.Brown
                Else
                    LabelInfoCIM.Text = "DOESN'T SEND CIM DATA"
                    LabelInfoCIM.BackColor = Color.Red
                End If
            End If
        End If
    End Sub

    Private Sub ManualToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManualToolStripMenuItem.Click
        GroupBoxManual.Visible = True
        GroupBoxProcessNest1.Visible = False
        GroupBoxProcessNest2.Visible = False
        GroupBoxProcessNest3.Visible = False
        GroupBoxProcessNest4.Visible = False
        GroupBoxBarcode.Visible = False
        Timer1.Enabled = False
        Timer2.Enabled = False
        AutoMode = False
    End Sub

    Private Sub FeasaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FeasaToolStripMenuItem.Click
        Feasa.Show()
    End Sub

    Private Sub ButtonCloseManual_Click(sender As Object, e As EventArgs) Handles ButtonCloseManual.Click
        GroupBoxManual.Visible = False
        GroupBoxProcessNest1.Visible = True
        GroupBoxProcessNest2.Visible = True
        GroupBoxProcessNest3.Visible = True
        GroupBoxProcessNest4.Visible = False
        GroupBoxBarcode.Visible = True
    End Sub

    Private Sub ButtonReset_Click(sender As Object, e As EventArgs) Handles ButtonReset.Click
        ClearPort()
        ResetIO()
    End Sub

    Private Sub ButtonWinCAL_Click(sender As Object, e As EventArgs) Handles ButtonWinCAL.Click
        OffComPWBAN1()
        OffComPWBAN2()
        OffComPWBAN3()
        OffComPWBAN4()
        Process.Start("C:\WinCAL\WinCal.exe")
    End Sub

    Private Sub CheckBoxJigNest1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxJigNest1.CheckedChanged
        If CheckBoxJigNest1.Checked = True Then
            JigN1On()
        Else
            JigN1Off()
        End If
    End Sub

    Private Sub CheckBoxBatteryNest1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxBatteryNest1.CheckedChanged
        If CheckBoxBatteryNest1.Checked = True Then
            PSU13V()
            Delay_Ms(25)
            BatteryN1On()
        Else
            BatteryN1Off()
        End If
    End Sub

    Private Sub CheckBoxIgnitionNest1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxIgnitionNest1.CheckedChanged
        If CheckBoxIgnitionNest1.Checked = True Then
            PSU13V()
            Delay_Ms(25)
            IgnitionN1On()
        Else
            IgnitionN1Off()
        End If
    End Sub

    Private Sub CheckBoxAllLEDNest1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxAllLEDNest1.CheckedChanged
        If CheckBoxAllLEDNest1.Checked = True Then
            AllLEDN1On()
        Else
            AllLEDN1Off()
        End If
    End Sub

    Private Sub CheckBoxAllUARTLEDNest1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxAllUARTLEDNest1.CheckedChanged
        If CheckBoxAllUARTLEDNest1.Checked = True Then
            AllUARTLEDN1On()
        Else
            AllUARTLEDN1Off()
        End If
    End Sub

    Private Sub CheckBoxEOLNest1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxEOLNest1.CheckedChanged
        If CheckBoxEOLNest1.Checked = True Then
            EOLN1On()
        Else
            EOLN1Off()
        End If
    End Sub

    Private Sub CheckBoxBootmodeNest1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxBootmodeNest1.CheckedChanged
        If CheckBoxBootmodeNest1.Checked = True Then
            BootmodeN1On()
            PSU13V()
        Else
            BootmodeN1Off()
        End If
    End Sub

    Private Sub CheckBoxCheckNVMNest1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxCheckNVMNest1.CheckedChanged
        If CheckBoxCheckNVMNest1.Checked = True Then
            BootmodeN1On()
            CheckNVMN1()
            TextBoxNVM.Text = "N1 : " & strhexbuffN1(4) & "." & strhexbuffN1(5) & "." & strhexbuffN1(6)
            TextBoxNVM.ForeColor = Color.Red
        Else
            TextBoxNVM.Clear()
            TextBoxNVM.ForeColor = Color.White
            BootmodeN1Off()
        End If
    End Sub

    Private Sub CheckBoxJigNest2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxJigNest2.CheckedChanged
        If CheckBoxJigNest2.Checked = True Then
            JigN2On()
        Else
            JigN2Off()
        End If
    End Sub

    Private Sub CheckBoxBatteryNest2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxBatteryNest2.CheckedChanged
        If CheckBoxBatteryNest2.Checked = True Then
            PSU13V()
            Delay_Ms(25)
            BatteryN2On()
        Else
            BatteryN2Off()
        End If
    End Sub

    Private Sub CheckBoxIgnitionNest2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxIgnitionNest2.CheckedChanged
        If CheckBoxIgnitionNest2.Checked = True Then
            PSU13V()
            Delay_Ms(25)
            IgnitionN2On()
        Else
            IgnitionN2Off()
        End If
    End Sub

    Private Sub CheckBoxAllLEDNest2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxAllLEDNest2.CheckedChanged
        If CheckBoxAllLEDNest2.Checked = True Then
            AllLEDN2On()
        Else
            AllLEDN2Off()
        End If
    End Sub

    Private Sub CheckBoxAllUARTLEDNest2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxAllUARTLEDNest2.CheckedChanged
        If CheckBoxAllUARTLEDNest2.Checked = True Then
            AllUARTLEDN2On()
        Else
            AllUARTLEDN2Off()
        End If
    End Sub

    Private Sub CheckBoxEOLNest2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxEOLNest2.CheckedChanged
        If CheckBoxEOLNest2.Checked = True Then
            EOLN2On()
        Else
            EOLN2Off()
        End If
    End Sub

    Private Sub CheckBoxBootmodeNest2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxBootmodeNest2.CheckedChanged
        If CheckBoxBootmodeNest2.Checked = True Then
            BootmodeN2On()
            PSU13V()
        Else
            BootmodeN2Off()
        End If
    End Sub

    Private Sub CheckBoxCheckNVMNest2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxCheckNVMNest2.CheckedChanged
        If CheckBoxCheckNVMNest2.Checked = True Then
            BootmodeN2On()
            CheckNVMN2()
            TextBoxNVM.Text = "N2 : " & strhexbuffN2(4) & "." & strhexbuffN2(5) & "." & strhexbuffN2(6)
            TextBoxNVM.ForeColor = Color.Red
        Else
            TextBoxNVM.Clear()
            TextBoxNVM.ForeColor = Color.White
            BootmodeN2Off()
        End If
    End Sub

    Private Sub CheckBoxJigNest3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxJigNest3.CheckedChanged
        If CheckBoxJigNest3.Checked = True Then
            JigN3On()
        Else
            JigN3Off()
        End If
    End Sub

    Private Sub CheckBoxBatteryNest3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxBatteryNest3.CheckedChanged
        If CheckBoxBatteryNest3.Checked = True Then
            PSU13V()
            Delay_Ms(25)
            BatteryN3On()
        Else
            BatteryN3Off()
        End If
    End Sub

    Private Sub CheckBoxIgnitionNest3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxIgnitionNest3.CheckedChanged
        If CheckBoxIgnitionNest3.Checked = True Then
            PSU13V()
            Delay_Ms(25)
            IgnitionN3On()
        Else
            IgnitionN3Off()
        End If
    End Sub

    Private Sub CheckBoxAllLEDNest3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxAllLEDNest3.CheckedChanged
        If CheckBoxAllLEDNest3.Checked = True Then
            AllLEDN3On()
        Else
            AllLEDN3Off()
        End If
    End Sub

    Private Sub CheckBoxAllUARTLEDNest3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxAllUARTLEDNest3.CheckedChanged
        If CheckBoxAllUARTLEDNest3.Checked = True Then
            AllUARTLEDN3On()
        Else
            AllUARTLEDN3Off()
        End If
    End Sub

    Private Sub CheckBoxEOLNest3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxEOLNest3.CheckedChanged
        If CheckBoxEOLNest3.Checked = True Then
            EOLN3On()
        Else
            EOLN3Off()
        End If
    End Sub

    Private Sub CheckBoxBootmodeNest3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxBootmodeNest3.CheckedChanged
        If CheckBoxBootmodeNest3.Checked = True Then
            BootmodeN3On()
            PSU13V()
        Else
            BootmodeN3Off()
        End If
    End Sub

    Private Sub CheckBoxCheckNVMNest3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxCheckNVMNest3.CheckedChanged
        If CheckBoxCheckNVMNest3.Checked = True Then
            BootmodeN3On()
            CheckNVMN3()
            TextBoxNVM.Text = "N3 : " & strhexbuffN3(4) & "." & strhexbuffN3(5) & "." & strhexbuffN3(6)
            TextBoxNVM.ForeColor = Color.Red
        Else
            TextBoxNVM.Clear()
            TextBoxNVM.ForeColor = Color.White
            BootmodeN3Off()
        End If
    End Sub

    Private Sub CheckBoxJigNest4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxJigNest4.CheckedChanged
        If CheckBoxJigNest4.Checked = True Then
            JigN4On()
        Else
            JigN4Off()
        End If
    End Sub

    Private Sub CheckBoxBatteryNest4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxBatteryNest4.CheckedChanged
        If CheckBoxBatteryNest4.Checked = True Then
            PSU13V()
            Delay_Ms(25)
            BatteryN4On()
        Else
            BatteryN4Off()
        End If
    End Sub

    Private Sub CheckBoxIgnitionNest4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxIgnitionNest4.CheckedChanged
        If CheckBoxIgnitionNest4.Checked = True Then
            PSU13V()
            Delay_Ms(25)
            IgnitionN4On()
        Else
            IgnitionN4Off()
        End If
    End Sub

    Private Sub CheckBoxAllLEDNest4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxAllLEDNest4.CheckedChanged
        If CheckBoxAllLEDNest4.Checked = True Then
            AllLEDN4On()
        Else
            AllLEDN4Off()
        End If
    End Sub

    Private Sub CheckBoxAllUARTLEDNest4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxAllUARTLEDNest4.CheckedChanged
        If CheckBoxAllUARTLEDNest4.Checked = True Then
            AllUARTLEDN4On()
        Else
            AllUARTLEDN4Off()
        End If
    End Sub

    Private Sub CheckBoxEOLNest4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxEOLNest4.CheckedChanged
        If CheckBoxEOLNest4.Checked = True Then
            EOLN4On()
        Else
            EOLN4Off()
        End If
    End Sub

    Private Sub CheckBoxBootmodeNest4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxBootmodeNest4.CheckedChanged
        If CheckBoxBootmodeNest4.Checked = True Then
            BootmodeN4On()
            PSU13V()
        Else
            BootmodeN4Off()
        End If
    End Sub

    Private Sub CheckBoxCheckNVMNest4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxCheckNVMNest4.CheckedChanged
        If CheckBoxCheckNVMNest4.Checked = True Then
            BootmodeN4On()
            CheckNVMN4()
            TextBoxNVM.Text = "N4 : " & strhexbuffN4(4) & "." & strhexbuffN4(5) & "." & strhexbuffN4(6)
            TextBoxNVM.ForeColor = Color.Red
        Else
            TextBoxNVM.Clear()
            TextBoxNVM.ForeColor = Color.White
            BootmodeN4Off()
        End If
    End Sub

    Public Sub JigN1On()
        CheckSafetySensor()
        If SafetySensor = True Then
            ValPort2CL = ValPort2CL Or &H1
            Call Wr_DoPort()
        End If
    End Sub

    Public Sub JigN1Off()
        ValPort2CL = ValPort2CL And &HFE
        Call Wr_DoPort()
    End Sub

    Public Sub IgnitionN1On()
        ValPort1B = ValPort1B Or &H80
        Call Wr_DoPort()
    End Sub

    Public Sub IgnitionN1Off()
        ValPort1B = ValPort1B And &H7F
        Call Wr_DoPort()
    End Sub

    Public Sub BatteryN1On()
        ValPort1CH = ValPort1CH Or &H8
        Call Wr_DoPort()
    End Sub

    Public Sub BatteryN1Off()
        ValPort1CH = ValPort1CH And &HF7
        Call Wr_DoPort()
    End Sub

    Public Sub SpeedABSN1On()
        ValPort1B = ValPort1B Or &H1
        Call Wr_DoPort()
    End Sub

    Public Sub SpeedABSN1Off()
        ValPort1B = ValPort1B And &HFE
        Call Wr_DoPort()
    End Sub

    Public Sub SpeedNABSN1On()
        ValPort1B = ValPort1B Or &H4
        Call Wr_DoPort()
    End Sub

    Public Sub SpeedNABSN1Off()
        ValPort1B = ValPort1B And &HFB
        Call Wr_DoPort()
    End Sub

    Public Sub EOLTXN1On()
        ValPort1CL = ValPort1CL Or &H1
        Call Wr_DoPort()
    End Sub

    Public Sub EOLTXN1Off()
        ValPort1CL = ValPort1CL And &HFE
        Call Wr_DoPort()
    End Sub

    Public Sub EOLRXN1On()
        ValPort1B = ValPort1B Or &H8
        Call Wr_DoPort()
    End Sub

    Public Sub EOLRXN1Off()
        ValPort1B = ValPort1B And &HF7
        Call Wr_DoPort()
    End Sub

    Public Sub EOLShortN1On()
        ValPort1A = ValPort1A Or &H1
        Call Wr_DoPort()
    End Sub

    Public Sub EOLShortN1Off()
        ValPort1A = ValPort1A And &HFE
        Call Wr_DoPort()
    End Sub

    Public Sub HighBeamN1On()
        ValPort1CL = ValPort1CL Or &H4
        Call Wr_DoPort()
    End Sub

    Public Sub HighBeamN1Off()
        ValPort1CL = ValPort1CL And &HFB
        Call Wr_DoPort()
    End Sub

    Public Sub ABSN1On()
        ValPort1CH = ValPort1CH Or &H4
        Call Wr_DoPort()
    End Sub

    Public Sub ABSN1Off()
        ValPort1CH = ValPort1CH And &HFB
        Call Wr_DoPort()
    End Sub

    Public Sub TurnLeftN1On()
        ValPort1CL = ValPort1CL Or &H8
        Call Wr_DoPort()
    End Sub

    Public Sub TurnLeftN1Off()
        ValPort1CL = ValPort1CL And &HF7
        Call Wr_DoPort()
    End Sub

    Public Sub AlarmN1On()
        ValPort1B = ValPort1B Or &H2
        Call Wr_DoPort()
    End Sub

    Public Sub AlarmN1Off()
        ValPort1B = ValPort1B And &HFD
        Call Wr_DoPort()
    End Sub

    Public Sub TurnRightN1On()
        ValPort1B = ValPort1B Or &H20
        Call Wr_DoPort()
    End Sub

    Public Sub TurnRightN1Off()
        ValPort1B = ValPort1B And &HDF
        Call Wr_DoPort()
    End Sub

    Public Sub SmartN1On()
        ValPort1B = ValPort1B Or &H40
        Call Wr_DoPort()
    End Sub

    Public Sub SmartN1Off()
        ValPort1B = ValPort1B And &HBF
        Call Wr_DoPort()
    End Sub

    Public Sub AllLEDN1On()
        AllUARTLEDN1On()
        TurnLeftN1On()
        SmartN1On()
        HighBeamN1On()
        TurnRightN1On()
        AlarmN1On()
        ABSN1On()
    End Sub

    Public Sub AllLEDN1Off()
        AllUARTLEDN1Off()
        TurnLeftN1Off()
        SmartN1Off()
        HighBeamN1Off()
        TurnRightN1Off()
        AlarmN1Off()
        ABSN1Off()
    End Sub

    Public Sub AllUARTLEDN1On()
        ValPort1A = ValPort1A Or &H2
        Call Wr_DoPort()
    End Sub

    Public Sub AllUARTLEDN1Off()
        ValPort1A = ValPort1A And &HFD
        Call Wr_DoPort()
    End Sub

    Public Sub EOLN1On()
        EOLShortN1On()
        Delay_Ms(50)
        'OnComPWBAN1()
        PSU15V()
        SpeedABSN1On()
        'Delay_Ms(50)
        SpeedNABSN1On()
        'Delay_Ms(50)
        EOLTXN1On()
        Delay_Ms(50)
        EOLRXN1On()
        Delay_Ms(50)
        IgnitionN1On()
        Delay_Ms(50)
    End Sub

    Public Sub EOLN1Off()
        OffComPWBAN1()
        SpeedABSN1Off()
        SpeedNABSN1Off()
        EOLTXN1Off()
        EOLRXN1Off()
        EOLShortN1Off()
        IgnitionN1Off()
    End Sub

    Public Sub BootmodeN1On()
        OnComPWBAN1()
        PSU15V()
        SpeedABSN1On()
        Delay_Ms(50)
        SpeedNABSN1On()
        Delay_Ms(50)
        EOLTXN1On()
        Delay_Ms(50)
        EOLRXN1On()
        Delay_Ms(50)
        EOLShortN1On()
        Delay_Ms(200)
        IgnitionN1On()
        Delay_Ms(200)
        EOLShortN1Off()
        Delay_Ms(200)
        SerialPWBANest1.Output = "E"
    End Sub

    Public Sub BootmodeNonPSUN1On()
        OnComPWBAN1()
        SpeedABSN1On()
        Delay_Ms(50)
        SpeedNABSN1On()
        Delay_Ms(50)
        EOLTXN1On()
        Delay_Ms(50)
        EOLRXN1On()
        Delay_Ms(50)
        EOLShortN1On()
        Delay_Ms(200)
        IgnitionN1On()
        Delay_Ms(200)
        EOLShortN1Off()
        Delay_Ms(200)
        SerialPWBANest1.Output = "E"
    End Sub

    Public Sub BootmodeAN1On()
        OnComPWBAN1()
        PSU15AV()
        SpeedABSN1On()
        Delay_Ms(50)
        SpeedNABSN1On()
        Delay_Ms(50)
        EOLTXN1On()
        Delay_Ms(50)
        EOLRXN1On()
        Delay_Ms(50)
        EOLShortN1On()
        Delay_Ms(200)
        IgnitionN1On()
        Delay_Ms(200)
        EOLShortN1Off()
        Delay_Ms(200)
        SerialPWBANest1.Output = "E"
    End Sub

    Public Sub BootmodeBN1On()
        OnComPWBAN1()
        PSU15BV()
        SpeedABSN1On()
        Delay_Ms(50)
        SpeedNABSN1On()
        Delay_Ms(50)
        EOLTXN1On()
        Delay_Ms(50)
        EOLRXN1On()
        Delay_Ms(50)
        EOLShortN1On()
        Delay_Ms(200)
        IgnitionN1On()
        Delay_Ms(200)
        EOLShortN1Off()
        Delay_Ms(200)
        SerialPWBANest1.Output = "E"
    End Sub

    Public Sub BootmodeN1Off()
        OffComPWBAN1()
        SpeedABSN1Off()
        Delay_Ms(50)
        SpeedNABSN1Off()
        Delay_Ms(50)
        EOLTXN1Off()
        Delay_Ms(50)
        EOLRXN1Off()
        Delay_Ms(50)
        EOLShortN1Off()
        Delay_Ms(50)
        IgnitionN1Off()
        Delay_Ms(50)
    End Sub

    Public Sub JigN2On()
        CheckSafetySensor()
        If SafetySensor = True Then
            ValPort2CL = ValPort2CL Or &H2
            Call Wr_DoPort()
        End If
    End Sub

    Public Sub JigN2Off()
        ValPort2CL = ValPort2CL And &HFD
        Call Wr_DoPort()
    End Sub

    Public Sub IgnitionN2On()
        RelayBoard(16, 1)
    End Sub

    Public Sub IgnitionN2Off()
        RelayBoard(16, 0)
    End Sub

    Public Sub BatteryN2On()
        RelayBoard(8, 1)
    End Sub

    Public Sub BatteryN2Off()
        RelayBoard(8, 0)
    End Sub

    Public Sub SpeedABSN2On()
        RelayBoard(9, 1)
    End Sub

    Public Sub SpeedABSN2Off()
        RelayBoard(9, 0)
    End Sub

    Public Sub SpeedNABSN2On()
        RelayBoard(11, 1)
    End Sub

    Public Sub SpeedNABSN2Off()
        RelayBoard(11, 0)
    End Sub

    Public Sub EOLTXN2On()
        RelayBoard(1, 1)
    End Sub

    Public Sub EOLTXN2Off()
        RelayBoard(1, 0)
    End Sub

    Public Sub EOLRXN2On()
        RelayBoard(12, 1)
    End Sub

    Public Sub EOLRXN2Off()
        RelayBoard(12, 0)
    End Sub

    Public Sub EOLShortN2On()
        RelayBoard(17, 1)
    End Sub

    Public Sub EOLShortN2Off()
        RelayBoard(17, 0)
    End Sub

    Public Sub HighBeamN2On()
        RelayBoard(3, 1)
    End Sub

    Public Sub HighBeamN2Off()
        RelayBoard(3, 0)
    End Sub

    Public Sub ABSN2On()
        RelayBoard(7, 1)
    End Sub

    Public Sub ABSN2Off()
        RelayBoard(7, 0)
    End Sub

    Public Sub TurnLeftN2On()
        RelayBoard(4, 1)
    End Sub

    Public Sub TurnLeftN2Off()
        RelayBoard(4, 0)
    End Sub

    Public Sub AlarmN2On()
        RelayBoard(10, 1)
    End Sub

    Public Sub AlarmN2Off()
        RelayBoard(10, 0)
    End Sub

    Public Sub TurnRightN2On()
        RelayBoard(14, 1)
    End Sub

    Public Sub TurnRightN2Off()
        RelayBoard(14, 0)
    End Sub

    Public Sub SmartN2On()
        RelayBoard(15, 1)
    End Sub

    Public Sub SmartN2Off()
        RelayBoard(15, 0)
    End Sub

    Public Sub AllLEDN2On()
        AllUARTLEDN2On()
        TurnLeftN2On()
        SmartN2On()
        HighBeamN2On()
        TurnRightN2On()
        AlarmN2On()
        ABSN2On()
    End Sub

    Public Sub AllLEDN2Off()
        AllUARTLEDN2Off()
        TurnLeftN2Off()
        SmartN2Off()
        HighBeamN2Off()
        TurnRightN2Off()
        AlarmN2Off()
        ABSN2Off()
    End Sub

    Public Sub AllUARTLEDN2On()
        RelayBoard(18, 1)
    End Sub

    Public Sub AllUARTLEDN2Off()
        RelayBoard(18, 0)
    End Sub

    Public Sub EOLN2On()
        EOLShortN2On()
        Delay_Ms(50)
        OnComPWBAN2()
        PSU15V()
        SpeedABSN2On()
        'Delay_Ms(50)
        SpeedNABSN2On()
        'Delay_Ms(50)
        EOLTXN2On()
        Delay_Ms(50)
        EOLRXN2On()
        Delay_Ms(50)
        IgnitionN2On()
        Delay_Ms(50)
    End Sub

    Public Sub EOLN2Off()
        OffComPWBAN2()
        SpeedABSN2Off()
        SpeedNABSN2Off()
        EOLTXN2Off()
        EOLRXN2Off()
        EOLShortN2Off()
        IgnitionN2Off()
    End Sub

    Public Sub BootmodeN2On()
        OnComPWBAN2()
        PSU15V()
        SpeedABSN2On()
        Delay_Ms(50)
        SpeedNABSN2On()
        Delay_Ms(50)
        EOLTXN2On()
        Delay_Ms(50)
        EOLRXN2On()
        Delay_Ms(50)
        EOLShortN2On()
        Delay_Ms(200)
        IgnitionN2On()
        Delay_Ms(200)
        EOLShortN2Off()
        Delay_Ms(200)
        SerialPWBANest2.Output = "E"
    End Sub

    Public Sub BootmodeNonPSUN2On()
        OnComPWBAN2()
        SpeedABSN2On()
        Delay_Ms(50)
        SpeedNABSN2On()
        Delay_Ms(50)
        EOLTXN2On()
        Delay_Ms(50)
        EOLRXN2On()
        Delay_Ms(50)
        EOLShortN2On()
        Delay_Ms(200)
        IgnitionN2On()
        Delay_Ms(200)
        EOLShortN2Off()
        Delay_Ms(200)
        SerialPWBANest2.Output = "E"
    End Sub

    Public Sub BootmodeAN2On()
        OnComPWBAN2()
        PSU15AV()
        SpeedABSN2On()
        Delay_Ms(50)
        SpeedNABSN2On()
        Delay_Ms(50)
        EOLTXN2On()
        Delay_Ms(50)
        EOLRXN2On()
        Delay_Ms(50)
        EOLShortN2On()
        Delay_Ms(200)
        IgnitionN2On()
        Delay_Ms(200)
        EOLShortN2Off()
        Delay_Ms(200)
        SerialPWBANest2.Output = "E"
    End Sub

    Public Sub BootmodeBN2On()
        OnComPWBAN2()
        PSU15BV()
        SpeedABSN2On()
        Delay_Ms(50)
        SpeedNABSN2On()
        Delay_Ms(50)
        EOLTXN2On()
        Delay_Ms(50)
        EOLRXN2On()
        Delay_Ms(50)
        EOLShortN2On()
        Delay_Ms(200)
        IgnitionN2On()
        Delay_Ms(200)
        EOLShortN2Off()
        Delay_Ms(200)
        SerialPWBANest2.Output = "E"
    End Sub

    Public Sub BootmodeN2Off()
        OffComPWBAN2()
        SpeedABSN2Off()
        Delay_Ms(50)
        SpeedNABSN2Off()
        Delay_Ms(50)
        EOLTXN2Off()
        Delay_Ms(50)
        EOLRXN2Off()
        Delay_Ms(50)
        EOLShortN2Off()
        Delay_Ms(50)
        IgnitionN2Off()
        Delay_Ms(50)
    End Sub

    Public Sub JigN3On()
        CheckSafetySensor()
        If SafetySensor = True Then
            ValPort2CL = ValPort2CL Or &H4
            Call Wr_DoPort()
        End If
    End Sub

    Public Sub JigN3Off()
        ValPort2CL = ValPort2CL And &HFB
        Call Wr_DoPort()
    End Sub

    Public Sub IgnitionN3On()
        RelayBoard(36, 1)
    End Sub

    Public Sub IgnitionN3Off()
        RelayBoard(36, 0)
    End Sub

    Public Sub BatteryN3On()
        RelayBoard(28, 1)
    End Sub

    Public Sub BatteryN3Off()
        RelayBoard(28, 0)
    End Sub

    Public Sub SpeedABSN3On()
        RelayBoard(29, 1)
    End Sub

    Public Sub SpeedABSN3Off()
        RelayBoard(29, 0)
    End Sub

    Public Sub SpeedNABSN3On()
        RelayBoard(31, 1)
    End Sub

    Public Sub SpeedNABSN3Off()
        RelayBoard(31, 0)
    End Sub

    Public Sub EOLTXN3On()
        RelayBoard(21, 1)
    End Sub

    Public Sub EOLTXN3Off()
        RelayBoard(21, 0)
    End Sub

    Public Sub EOLRXN3On()
        RelayBoard(32, 1)
    End Sub

    Public Sub EOLRXN3Off()
        RelayBoard(32, 0)
    End Sub

    Public Sub EOLShortN3On()
        RelayBoard(37, 1)
    End Sub

    Public Sub EOLShortN3Off()
        RelayBoard(37, 0)
    End Sub

    Public Sub HighBeamN3On()
        RelayBoard(23, 1)
    End Sub

    Public Sub HighBeamN3Off()
        RelayBoard(23, 0)
    End Sub

    Public Sub ABSN3On()
        RelayBoard(27, 1)
    End Sub

    Public Sub ABSN3Off()
        RelayBoard(27, 0)
    End Sub

    Public Sub TurnLeftN3On()
        RelayBoard(24, 1)
    End Sub

    Public Sub TurnLeftN3Off()
        RelayBoard(24, 0)
    End Sub

    Public Sub AlarmN3On()
        RelayBoard(30, 1)
    End Sub

    Public Sub AlarmN3Off()
        RelayBoard(30, 0)
    End Sub

    Public Sub TurnRightN3On()
        RelayBoard(34, 1)
    End Sub

    Public Sub TurnRightN3Off()
        RelayBoard(34, 0)
    End Sub

    Public Sub SmartN3On()
        RelayBoard(35, 1)
    End Sub

    Public Sub SmartN3Off()
        RelayBoard(35, 0)
    End Sub

    Public Sub AllLEDN3On()
        AllUARTLEDN3On()
        TurnLeftN3On()
        SmartN3On()
        HighBeamN3On()
        TurnRightN3On()
        AlarmN3On()
        ABSN3On()
    End Sub

    Public Sub AllLEDN3Off()
        AllUARTLEDN3Off()
        TurnLeftN3Off()
        SmartN3Off()
        HighBeamN3Off()
        TurnRightN3Off()
        AlarmN3Off()
        ABSN3Off()
    End Sub

    Public Sub AllUARTLEDN3On()
        RelayBoard(38, 1)
    End Sub

    Public Sub AllUARTLEDN3Off()
        RelayBoard(38, 0)
    End Sub

    Public Sub EOLN3On()
        PSU15V()
        'ABSN3On()
        'Delay_Ms(50)
        'SmartN3On()
        'Delay_Ms(50)
        EOLShortN3On()
        Delay_Ms(50)
        'OnComPWBAN3()
        SpeedABSN3On()
        'Delay_Ms(50)
        SpeedNABSN3On()
        'Delay_Ms(50)
        EOLTXN3On()
        Delay_Ms(50)
        EOLRXN3On()
        Delay_Ms(50)
        IgnitionN3On()
        Delay_Ms(50)
    End Sub

    Public Sub EOLN3Off()
        'OffComPWBAN3()
        SpeedABSN3Off()
        Delay_Ms(50)
        SpeedNABSN3Off()
        Delay_Ms(50)
        EOLTXN3Off()
        Delay_Ms(50)
        EOLRXN3Off()
        Delay_Ms(50)
        EOLShortN3Off()
        Delay_Ms(50)
        IgnitionN3Off()
        Delay_Ms(50)
    End Sub

    Public Sub BootmodeN3On()
        OnComPWBAN3()
        PSU15V()
        SpeedABSN3On()
        Delay_Ms(50)
        SpeedNABSN3On()
        Delay_Ms(50)
        EOLTXN3On()
        Delay_Ms(50)
        EOLRXN3On()
        Delay_Ms(50)
        EOLShortN3On()
        Delay_Ms(200)
        IgnitionN3On()
        Delay_Ms(200)
        EOLShortN3Off()
        Delay_Ms(200)
        SerialPWBANest3.Output = "E"
    End Sub

    Public Sub BootmodeNonPSUN3On()
        OnComPWBAN3()
        SpeedABSN3On()
        Delay_Ms(50)
        SpeedNABSN3On()
        Delay_Ms(50)
        EOLTXN3On()
        Delay_Ms(50)
        EOLRXN3On()
        Delay_Ms(50)
        EOLShortN3On()
        Delay_Ms(200)
        IgnitionN3On()
        Delay_Ms(200)
        EOLShortN3Off()
        Delay_Ms(200)
        SerialPWBANest3.Output = "E"
    End Sub

    Public Sub BootmodeAN3On()
        OnComPWBAN3()
        PSU15AV()
        SpeedABSN3On()
        Delay_Ms(50)
        SpeedNABSN3On()
        Delay_Ms(50)
        EOLTXN3On()
        Delay_Ms(50)
        EOLRXN3On()
        Delay_Ms(50)
        EOLShortN3On()
        Delay_Ms(200)
        IgnitionN3On()
        Delay_Ms(200)
        EOLShortN3Off()
        Delay_Ms(200)
        SerialPWBANest3.Output = "E"
    End Sub

    Public Sub BootmodeBN3On()
        OnComPWBAN3()
        PSU15BV()
        SpeedABSN3On()
        Delay_Ms(50)
        SpeedNABSN3On()
        Delay_Ms(50)
        EOLTXN3On()
        Delay_Ms(50)
        EOLRXN3On()
        Delay_Ms(50)
        EOLShortN3On()
        Delay_Ms(200)
        IgnitionN3On()
        Delay_Ms(200)
        EOLShortN3Off()
        Delay_Ms(200)
        SerialPWBANest3.Output = "E"
    End Sub

    Public Sub BootmodeN3Off()
        OffComPWBAN3()
        SpeedABSN3Off()
        Delay_Ms(50)
        SpeedNABSN3Off()
        Delay_Ms(50)
        EOLTXN3Off()
        Delay_Ms(50)
        EOLRXN3Off()
        Delay_Ms(50)
        EOLShortN3Off()
        Delay_Ms(50)
        IgnitionN3Off()
        Delay_Ms(50)
    End Sub

    Public Sub JigN4On()
        CheckSafetySensor()
        If SafetySensor = True Then
            ValPort2CL = ValPort2CL Or &H8
            Call Wr_DoPort()
        End If
    End Sub

    Public Sub JigN4Off()
        ValPort2CL = ValPort2CL And &HF7
        Call Wr_DoPort()
    End Sub

    Public Sub IgnitionN4On()
        RelayBoard(24, 1)
    End Sub

    Public Sub IgnitionN4Off()
        RelayBoard(24, 0)
    End Sub

    Public Sub BatteryN4On()
        RelayBoard(17, 1)
    End Sub

    Public Sub BatteryN4Off()
        RelayBoard(17, 0)
    End Sub

    Public Sub SpeedABSN4On()
        RelayBoard(18, 1)
    End Sub

    Public Sub SpeedABSN4Off()
        RelayBoard(18, 0)
    End Sub

    Public Sub SpeedNABSN4On()
        RelayBoard(20, 1)
    End Sub

    Public Sub SpeedNABSN4Off()
        RelayBoard(20, 0)
    End Sub

    Public Sub EOLTXN4On()
        RelayBoard(13, 1)
    End Sub

    Public Sub EOLTXN4Off()
        RelayBoard(13, 0)
    End Sub

    Public Sub EOLRXN4On()
        RelayBoard(21, 1)
    End Sub

    Public Sub EOLRXN4Off()
        RelayBoard(21, 0)
    End Sub

    Public Sub EOLShortN4On()
        RelayBoard(26, 1)
    End Sub

    Public Sub EOLShortN4Off()
        RelayBoard(26, 0)
    End Sub

    Public Sub HighBeamN4On()
        RelayBoard(14, 1)
    End Sub

    Public Sub HighBeamN4Off()
        RelayBoard(14, 0)
    End Sub

    Public Sub ABSN4On()
        RelayBoard(16, 1)
    End Sub

    Public Sub ABSN4Off()
        RelayBoard(16, 0)
    End Sub

    Public Sub TurnLeftN4On()
        RelayBoard(15, 1)
    End Sub

    Public Sub TurnLeftN4Off()
        RelayBoard(15, 0)
    End Sub

    Public Sub AlarmN4On()
        RelayBoard(19, 1)
    End Sub

    Public Sub AlarmN4Off()
        RelayBoard(19, 0)
    End Sub

    Public Sub TurnRightN4On()
        RelayBoard(22, 1)
    End Sub

    Public Sub TurnRightN4Off()
        RelayBoard(22, 0)
    End Sub

    Public Sub SmartN4On()
        RelayBoard(23, 1)
    End Sub

    Public Sub SmartN4Off()
        RelayBoard(23, 0)
    End Sub

    Public Sub AllLEDN4On()
        TurnLeftN4On()
        SmartN4On()
        HighBeamN4On()
        TurnRightN4On()
        ABSN4On()
        AlarmN4On()
        AllUARTLEDN4On()
    End Sub

    Public Sub AllLEDN4Off()
        TurnLeftN4Off()
        SmartN4Off()
        HighBeamN4Off()
        TurnRightN4Off()
        ABSN4Off()
        AlarmN4Off()
        AllUARTLEDN4Off()
    End Sub

    Public Sub UARTLED1N4On()
        RelayBoard(31, 1)
    End Sub

    Public Sub UARTLED1N4Off()
        RelayBoard(31, 0)
    End Sub

    Public Sub UARTLED2N4On()
        RelayBoard(32, 1)
    End Sub

    Public Sub UARTLED2N4Off()
        RelayBoard(32, 0)
    End Sub

    Public Sub UARTLED3N4On()
        RelayBoard(33, 1)
    End Sub

    Public Sub UARTLED3N4Off()
        RelayBoard(33, 0)
    End Sub

    Public Sub UARTLED4N4On()
        RelayBoard(34, 1)
    End Sub

    Public Sub UARTLED4N4Off()
        RelayBoard(34, 0)
    End Sub

    Public Sub AllUARTLEDN4On()
        UARTLED1N4On()
        UARTLED2N4On()
        UARTLED3N4On()
        UARTLED4N4On()
    End Sub

    Public Sub AllUARTLEDN4Off()
        UARTLED1N4Off()
        UARTLED2N4Off()
        UARTLED3N4Off()
        UARTLED4N4Off()
    End Sub

    Public Sub EOLN4On()
        'OnComPWBAN4()
        PSU15V()
        SpeedABSN4On()
        'Delay_Ms(50)
        SpeedNABSN4On()
        'Delay_Ms(50)
        EOLTXN4On()
        'Delay_Ms(50)
        EOLRXN4On()
        'Delay_Ms(50)
        EOLShortN4On()
        Delay_Ms(300)
        IgnitionN4On()
        Delay_Ms(50)
    End Sub

    Public Sub EOLN4Off()
        OffComPWBAN4()
        SpeedABSN4Off()
        SpeedNABSN4Off()
        EOLTXN4Off()
        EOLRXN4Off()
        EOLShortN4Off()
        IgnitionN4Off()
    End Sub

    Public Sub BootmodeN4On()
        OnComPWBAN4()
        PSU15V()
        SpeedABSN4On()
        Delay_Ms(50)
        SpeedNABSN4On()
        Delay_Ms(50)
        EOLTXN4On()
        Delay_Ms(50)
        EOLRXN4On()
        Delay_Ms(50)
        EOLShortN4On()
        Delay_Ms(200)
        IgnitionN4On()
        Delay_Ms(200)
        EOLShortN4Off()
        Delay_Ms(200)
        SerialPWBANest4.Output = "E"
    End Sub

    Public Sub BootmodeNonPSUN4On()
        OnComPWBAN4()
        SpeedABSN4On()
        Delay_Ms(50)
        SpeedNABSN4On()
        Delay_Ms(50)
        EOLTXN4On()
        Delay_Ms(50)
        EOLRXN4On()
        Delay_Ms(50)
        EOLShortN4On()
        Delay_Ms(200)
        IgnitionN4On()
        Delay_Ms(200)
        EOLShortN4Off()
        Delay_Ms(200)
        SerialPWBANest4.Output = "E"
    End Sub

    Public Sub BootmodeAN4On()
        OnComPWBAN4()
        PSU15AV()
        SpeedABSN4On()
        Delay_Ms(50)
        SpeedNABSN4On()
        Delay_Ms(50)
        EOLTXN4On()
        Delay_Ms(50)
        EOLRXN4On()
        Delay_Ms(50)
        EOLShortN4On()
        Delay_Ms(200)
        IgnitionN4On()
        Delay_Ms(200)
        EOLShortN4Off()
        Delay_Ms(200)
        SerialPWBANest4.Output = "E"
    End Sub

    Public Sub BootmodeBN4On()
        OnComPWBAN4()
        PSU15BV()
        SpeedABSN4On()
        Delay_Ms(50)
        SpeedNABSN4On()
        Delay_Ms(50)
        EOLTXN4On()
        Delay_Ms(50)
        EOLRXN4On()
        Delay_Ms(50)
        EOLShortN4On()
        Delay_Ms(200)
        IgnitionN4On()
        Delay_Ms(200)
        EOLShortN4Off()
        Delay_Ms(200)
        SerialPWBANest4.Output = "E"
    End Sub

    Public Sub BootmodeN4Off()
        OffComPWBAN4()
        SpeedABSN4Off()
        Delay_Ms(50)
        SpeedNABSN4Off()
        Delay_Ms(50)
        EOLTXN4Off()
        Delay_Ms(50)
        EOLRXN4Off()
        Delay_Ms(50)
        EOLShortN4Off()
        Delay_Ms(50)
        IgnitionN4Off()
        Delay_Ms(50)
    End Sub

    '===== SAFETY SENSOR =====

    Public Sub CheckSafetySensor()
        Rd_DiPort()
        If (ValPort3CH And &H8) = 0 Then
            System.Windows.Forms.Application.DoEvents()
            SafetySensor = False
        Else
            SafetySensor = True
        End If
    End Sub

    '===== TIMER =====

    Private Sub CheckBoxCheckInput_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxCheckInput.CheckedChanged
        If CheckBoxCheckInput.Checked = True Then
            Timer4.Enabled = True
        Else
            Timer4.Enabled = False
            TextBoxManualCheckInput.Clear()
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        If CaseElseCondition = False Then
            ProcessStep()
            InputCheck()
            Timer1.Enabled = True
        Else
            Timer1.Enabled = False
            Timer2.Enabled = True
        End If
    End Sub

    Private Sub InputCheck()
        TextBoxModel.Text = ComboBoxModel.Text

        Rd_DiPort()

        TextBoxInputDecimal.Text = ValPort3CL
        TextBoxInput.Text = Convert.ToString(ValPort3CL, 2).PadLeft(4, "0"c)

        If CheckBoxUseNest1.Checked = True Then
            RunningNest1 = True
        Else
            RunningNest1 = False
            TextBoxBarcodeN1.BackColor = Color.Orange
            TextBoxBarcodeN1.Text = "NOT USED"
            LabelJudgementN1.BackColor = Color.Orange
            LabelJudgementN1.Text = "NOT USED"
        End If

        If CheckBoxUseNest2.Checked = True Then
            RunningNest2 = True
        Else
            RunningNest2 = False
            TextBoxBarcodeN2.BackColor = Color.Orange
            TextBoxBarcodeN2.Text = "NOT USED"
            LabelJudgementN2.BackColor = Color.Orange
            LabelJudgementN2.Text = "NOT USED"
        End If

        If CheckBoxUseNest3.Checked = True Then
            RunningNest3 = True
        Else
            RunningNest3 = False
            TextBoxBarcodeN3.BackColor = Color.Orange
            TextBoxBarcodeN3.Text = "NOT USED"
            LabelJudgementN3.BackColor = Color.Orange
            LabelJudgementN3.Text = "NOT USED"
        End If

        If CheckBoxUseNest4.Checked = True Then
            RunningNest4 = True
        Else
            RunningNest4 = False
            TextBoxBarcodeN4.BackColor = Color.Orange
            TextBoxBarcodeN4.Text = "NOT USED"
            LabelJudgementN4.BackColor = Color.Orange
            LabelJudgementN4.Text = "NOT USED"
        End If

        If (ValPort3CL And &H2) = 0 Then
            PushButtonLeft = True
        Else
            PushButtonLeft = False
        End If

        If (ValPort3CL And &H4) <> 0 Then
            System.Windows.Forms.Application.DoEvents()
            PushButtonEmergency = False
            TextBoxInfo.Text = "EMERGENCY BUTTON PRESSED!"
            Timer3.Stop()
            Me.Stopwatch.Stop()
        Else
            PushButtonEmergency = True
        End If

        If (ValPort3CL And &H1) = 0 Then
            PushButtonRight = True
        Else
            PushButtonRight = False
        End If

        If PushButtonLeft = True And PushButtonEmergency = True And PushButtonRight = True And SafetySensor = True And (RequirePostBarcodeStart = False Or Barcode_Ready_Auto = True) Then
            ButtonReady = True
        Else
            ButtonReady = False
        End If

        If ComboBoxModel.Text = "" Then
            TextBoxInfo.Text = "CHOOSE MODEL!"
            SelectModel = False
            TextBoxBarcode.ReadOnly = True
        Else
            Model = ComboBoxModel.Text
            TextBoxBarcode.ReadOnly = False
            SelectModel = True
        End If

        If SelectModel = True And Len(TextBoxBarcode.Text) = 16 And PushButtonEmergency = True Then
            If RunningNest1 = True And RunningNest2 = True And RunningNest3 = True And RunningNest4 = True Then
                '===== Nest 1, 2, 3, 4 ====='
                Condition1 = True
                Condition2 = True
                Condition3 = True
                Condition4 = True
            ElseIf RunningNest1 = True And RunningNest2 = True And RunningNest3 = True And RunningNest4 = False Then
                '===== Nest 1, 2, 3 ====='
                Condition1 = True
                Condition2 = True
                Condition3 = True
                Condition4 = False
            ElseIf RunningNest1 = True And RunningNest2 = True And RunningNest3 = False And RunningNest4 = True Then
                '===== Nest 1, 2, 4 ====='
                Condition1 = True
                Condition2 = True
                Condition3 = False
                Condition4 = True
            ElseIf RunningNest1 = True And RunningNest2 = False And RunningNest3 = True And RunningNest4 = True Then
                '===== Nest 1, 3, 4 ====='
                Condition1 = True
                Condition2 = False
                Condition3 = True
                Condition4 = True
            ElseIf RunningNest1 = False And RunningNest2 = True And RunningNest3 = True And RunningNest4 = True Then
                '===== Nest 2, 3, 4 ====='
                Condition1 = False
                Condition2 = True
                Condition3 = True
                Condition4 = True
            ElseIf RunningNest1 = True And RunningNest2 = True And RunningNest3 = False And RunningNest4 = False Then
                '===== Nest 1, 2 ====='
                Condition1 = True
                Condition2 = True
                Condition3 = False
                Condition4 = False
            ElseIf RunningNest1 = True And RunningNest2 = False And RunningNest3 = True And RunningNest4 = False Then
                '===== Nest 1, 3 ====='
                Condition1 = True
                Condition2 = False
                Condition3 = True
                Condition4 = False
            ElseIf RunningNest1 = True And RunningNest2 = False And RunningNest3 = False And RunningNest4 = True Then
                '===== Nest 1, 4 ====='
                Condition1 = True
                Condition2 = False
                Condition3 = False
                Condition4 = True
            ElseIf RunningNest1 = False And RunningNest2 = True And RunningNest3 = True And RunningNest4 = False Then
                '===== Nest 2, 3 ====='
                Condition1 = False
                Condition2 = True
                Condition3 = True
                Condition4 = False
            ElseIf RunningNest1 = False And RunningNest2 = True And RunningNest3 = False And RunningNest4 = True Then
                '===== Nest 2, 4 ====='
                Condition1 = False
                Condition2 = True
                Condition3 = False
                Condition4 = True
            ElseIf RunningNest1 = False And RunningNest2 = False And RunningNest3 = True And RunningNest4 = True Then
                '===== Nest 3, 4 ====='
                Condition1 = False
                Condition2 = False
                Condition3 = True
                Condition4 = True
            ElseIf RunningNest1 = True And RunningNest2 = False And RunningNest3 = False And RunningNest4 = False Then
                '===== Nest 1 ====='
                Condition1 = True
                Condition2 = False
                Condition3 = False
                Condition4 = False
            ElseIf RunningNest1 = False And RunningNest2 = True And RunningNest3 = False And RunningNest4 = False Then
                '===== Nest 2 =====
                Condition1 = False
                Condition2 = True
                Condition3 = False
                Condition4 = False
            ElseIf RunningNest1 = False And RunningNest2 = False And RunningNest3 = True And RunningNest4 = False Then
                '===== Nest 3 ====='
                Condition1 = False
                Condition2 = False
                Condition3 = True
                Condition4 = False
            ElseIf RunningNest1 = False And RunningNest2 = False And RunningNest3 = False And RunningNest4 = True Then
                '===== Nest 4 ====='
                Condition1 = False
                Condition2 = False
                Condition3 = False
                Condition4 = True
            End If

            If Condition1 = True And Condition2 = True And Condition3 = True And Condition4 = True Then
                '===== Nest 1, 2, 3, 4 ====='
                Model_SelectModelN1 = False
                Model_SelectModelN2 = False
                Model_SelectModelN3 = False
                Model_SelectModelN4 = False
                If BarcodeStep1 = False And BarcodeStep2 = False And BarcodeStep3 = False Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN1 = TextBoxBarcode.Text
                    ModelN1 = Microsoft.VisualBasic.Mid(BarcodeN1, 1, 2)

                    If Model = ModelN1 Then
                        Model_SelectModelN1 = True
                    Else
                        Model_SelectModelN1 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                    End If

                    If Model_SelectModelN1 = True Then
                        SelectModelN1 = True
                        TextBoxBarcodeN1.ReadOnly = True
                    Else
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                        SelectModelN1 = False
                    End If

                    If SelectModelN1 = True Then
                        If CheckBoxSendCIM.Checked = True Then
                            Write101N1()
                            Delay_Ms(200)
                            Read102()
                            Delay_Ms(50)
                        Else
                            If CIMRun = False Then
                                CIMRun = True
                                LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                            End If
                        End If
                    End If

                    CheckSafetySensor()

                    If CIMRun = True And SelectModelN1 = True And SafetySensor = True Then
                        TextBoxBarcodeN1.Text = BarcodeN1
                        TextBoxBarcodeN1.BackColor = Color.Cyan

                        For i = 0 To 40
                            For j = 1 To 5
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Value = ""
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                            Next
                        Next

                        BarcodeStep1 = True

                        JigN1Off()
                        JigN2On()
                        JigN3On()
                        JigN4On()

                        TextBoxBarcode.Clear()
                    ElseIf CIMRun = False Then
                        TextBoxInfo.Text = "CHECK CIM HISTORY!"
                        TextBoxBarcode.Clear()
                    ElseIf SelectModelN1 = False Then
                        TextBoxInfo.Text = "CHECK MODEL!"
                        TextBoxBarcode.Clear()
                    ElseIf SafetySensor = False Then
                        TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                        TextBoxBarcode.Clear()
                    End If
                End If

                If BarcodeStep1 = True And BarcodeStep2 = False And BarcodeStep3 = False And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN2 = TextBoxBarcode.Text
                    ModelN2 = Microsoft.VisualBasic.Mid(BarcodeN2, 1, 2)

                    If Model = ModelN2 Then
                        Model_SelectModelN2 = True
                    Else
                        Model_SelectModelN2 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                    End If

                    If Model_SelectModelN2 = True Then
                        SelectModelN2 = True
                        TextBoxBarcodeN2.ReadOnly = True
                    Else
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                        SelectModelN2 = False
                    End If

                    If BarcodeN2 = BarcodeN1 Then
                        BarcodeN2 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                    Else
                        If SelectModelN2 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N2()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN2 = True And SafetySensor = True Then
                            TextBoxBarcodeN2.Text = BarcodeN2
                            TextBoxBarcodeN2.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest2.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest2.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep1 = False
                            BarcodeStep2 = True

                            JigN1On()
                            JigN2Off()
                            JigN3On()
                            JigN4On()

                            TextBoxBarcode.Clear()
                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN2 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

                If BarcodeStep1 = False And BarcodeStep2 = True And BarcodeStep3 = False And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN3 = TextBoxBarcode.Text
                    ModelN3 = Microsoft.VisualBasic.Mid(BarcodeN3, 1, 2)

                    If Model = ModelN3 Then
                        Model_SelectModelN3 = True
                    Else
                        Model_SelectModelN3 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                    End If

                    If Model_SelectModelN3 = True Then
                        SelectModelN3 = True
                        TextBoxBarcodeN3.ReadOnly = True
                    Else
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                        SelectModelN3 = False
                    End If

                    If BarcodeN3 = BarcodeN2 Or BarcodeN3 = BarcodeN1 Then
                        BarcodeN3 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                    Else
                        If SelectModelN3 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N3()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN3 = True And SafetySensor = True Then
                            TextBoxBarcodeN3.Text = BarcodeN3
                            TextBoxBarcodeN3.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest3.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest3.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep2 = False
                            BarcodeStep3 = True

                            JigN1On()
                            JigN2On()
                            JigN3Off()
                            JigN4On()

                            TextBoxBarcode.Clear()
                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN3 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK CIM SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

                If BarcodeStep1 = False And BarcodeStep2 = False And BarcodeStep3 = True And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN4 = TextBoxBarcode.Text
                    ModelN4 = Microsoft.VisualBasic.Mid(BarcodeN4, 1, 2)

                    If Model = ModelN4 Then
                        Model_SelectModelN4 = True
                    Else
                        Model_SelectModelN4 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                    End If

                    If Model_SelectModelN4 = True Then
                        SelectModelN4 = True
                        TextBoxBarcodeN4.ReadOnly = True
                    Else
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                        SelectModelN4 = False
                    End If

                    If BarcodeN4 = BarcodeN3 Or BarcodeN4 = BarcodeN2 Or BarcodeN4 = BarcodeN1 Then
                        BarcodeN4 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                    Else
                        If SelectModelN4 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N4()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN4 = True And SafetySensor = True Then
                            TextBoxBarcodeN4.Text = BarcodeN4
                            TextBoxBarcodeN4.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep3 = False

                            JigN1On()
                            JigN2On()
                            JigN3On()
                            JigN4Off()

                            TextBoxBarcode.Clear()
                            Barcode_Ready_Auto = True

                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN4 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

            ElseIf Condition1 = True And Condition2 = True And Condition3 = True And Condition4 = False Then
                '===== Nest 1, 2, 3 ====='
                Model_SelectModelN1 = False
                Model_SelectModelN2 = False
                Model_SelectModelN3 = False

                If BarcodeStep1 = False And BarcodeStep2 = False Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN1 = TextBoxBarcode.Text
                    ModelN1 = Microsoft.VisualBasic.Mid(BarcodeN1, 1, 2)

                    If Model = ModelN1 Then
                        Model_SelectModelN1 = True
                    Else
                        Model_SelectModelN1 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                    End If

                    If Model_SelectModelN1 = True Then
                        SelectModelN1 = True
                        TextBoxBarcodeN1.ReadOnly = True
                    Else
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                        SelectModelN1 = False
                    End If

                    If SelectModelN1 = True Then
                        If CheckBoxSendCIM.Checked = True Then
                            Write101N1()
                            Delay_Ms(200)
                            Read102()
                            Delay_Ms(50)
                        Else
                            If CIMRun = False Then
                                CIMRun = True
                                LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                            End If
                        End If
                    End If

                    CheckSafetySensor()

                    If CIMRun = True And SelectModelN1 = True And SafetySensor = True Then
                        TextBoxBarcodeN1.Text = BarcodeN1
                        TextBoxBarcodeN1.BackColor = Color.Cyan

                        For i = 0 To 40
                            For j = 1 To 5
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Value = ""
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                            Next
                        Next

                        BarcodeStep1 = True

                        JigN1Off()
                        JigN2On()
                        JigN3On()
                        JigN4On()

                        TextBoxBarcode.Clear()
                    ElseIf CIMRun = False Then
                        TextBoxInfo.Text = "CHECK CIM HISTORY!"
                        TextBoxBarcode.Clear()
                    ElseIf SelectModelN1 = False Then
                        TextBoxInfo.Text = "CHECK MODEL!"
                        TextBoxBarcode.Clear()
                    ElseIf SafetySensor = False Then
                        TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                        TextBoxBarcode.Clear()
                    End If
                End If

                If BarcodeStep1 = True And BarcodeStep2 = False And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN2 = TextBoxBarcode.Text
                    ModelN2 = Microsoft.VisualBasic.Mid(BarcodeN2, 1, 2)

                    If Model = ModelN2 Then
                        Model_SelectModelN2 = True
                    Else
                        Model_SelectModelN2 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                    End If

                    If Model_SelectModelN2 = True Then
                        SelectModelN2 = True
                        TextBoxBarcodeN2.ReadOnly = True
                    Else
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                        SelectModelN2 = False
                    End If

                    If BarcodeN2 = BarcodeN1 Then
                        BarcodeN2 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                    Else
                        If SelectModelN2 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N2()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN2 = True And SafetySensor = True Then
                            TextBoxBarcodeN2.Text = BarcodeN2
                            TextBoxBarcodeN2.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest2.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest2.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep1 = False
                            BarcodeStep2 = True

                            JigN1On()
                            JigN2Off()
                            JigN3On()
                            JigN4On()

                            TextBoxBarcode.Clear()
                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN2 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

                If BarcodeStep1 = False And BarcodeStep2 = True And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN3 = TextBoxBarcode.Text
                    ModelN3 = Microsoft.VisualBasic.Mid(BarcodeN3, 1, 2)

                    If Model = ModelN3 Then
                        Model_SelectModelN3 = True
                    Else
                        Model_SelectModelN3 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                    End If

                    If Model_SelectModelN3 = True Then
                        SelectModelN3 = True
                        TextBoxBarcodeN3.ReadOnly = True
                    Else
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                        SelectModelN3 = False
                    End If

                    If BarcodeN3 = BarcodeN2 Or BarcodeN3 = BarcodeN1 Then
                        BarcodeN3 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                    Else
                        If SelectModelN3 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N3()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN3 = True And SafetySensor = True Then
                            TextBoxBarcodeN3.Text = BarcodeN3
                            TextBoxBarcodeN3.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest3.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest3.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep2 = False

                            JigN1On()
                            JigN2On()
                            JigN3Off()
                            JigN4On()

                            TextBoxBarcode.Clear()
                            Barcode_Ready_Auto = True

                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN3 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

            ElseIf Condition1 = True And Condition2 = True And Condition3 = False And Condition4 = True Then
                '===== Nest 1, 2, 4 ====='
                Model_SelectModelN1 = False
                Model_SelectModelN2 = False
                Model_SelectModelN4 = False

                If BarcodeStep1 = False And BarcodeStep2 = False Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN1 = TextBoxBarcode.Text
                    ModelN1 = Microsoft.VisualBasic.Mid(BarcodeN1, 1, 2)

                    If Model = ModelN1 Then
                        Model_SelectModelN1 = True
                    Else
                        Model_SelectModelN1 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                    End If

                    If Model_SelectModelN1 = True Then
                        SelectModelN1 = True
                        TextBoxBarcodeN1.ReadOnly = True
                    Else
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                        SelectModelN1 = False
                    End If

                    If SelectModelN1 = True Then
                        If CheckBoxSendCIM.Checked = True Then
                            Write101N1()
                            Delay_Ms(200)
                            Read102()
                            Delay_Ms(50)
                        Else
                            If CIMRun = False Then
                                CIMRun = True
                                LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                            End If
                        End If
                    End If

                    CheckSafetySensor()

                    If CIMRun = True And SelectModelN1 = True And SafetySensor = True Then
                        TextBoxBarcodeN1.Text = BarcodeN1
                        TextBoxBarcodeN1.BackColor = Color.Cyan

                        For i = 0 To 40
                            For j = 1 To 5
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Value = ""
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                            Next
                        Next

                        BarcodeStep1 = True

                        JigN1Off()
                        JigN2On()
                        JigN3On()
                        JigN4On()

                        TextBoxBarcode.Clear()
                    ElseIf CIMRun = False Then
                        TextBoxInfo.Text = "CHECK CIM HISTORY!"
                        TextBoxBarcode.Clear()
                    ElseIf SelectModelN1 = False Then
                        TextBoxInfo.Text = "CHECK MODEL!"
                        TextBoxBarcode.Clear()
                    ElseIf SafetySensor = False Then
                        TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                        TextBoxBarcode.Clear()
                    End If
                End If

                If BarcodeStep1 = True And BarcodeStep2 = False And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN2 = TextBoxBarcode.Text
                    ModelN2 = Microsoft.VisualBasic.Mid(BarcodeN2, 1, 2)

                    If Model = ModelN2 Then
                        Model_SelectModelN2 = True
                    Else
                        Model_SelectModelN2 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                    End If

                    If Model_SelectModelN2 = True Then
                        SelectModelN2 = True
                        TextBoxBarcodeN2.ReadOnly = True
                    Else
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                        SelectModelN2 = False
                    End If

                    If BarcodeN2 = BarcodeN1 Then
                        BarcodeN2 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                    Else
                        If SelectModelN2 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N2()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN2 = True And SafetySensor = True Then
                            TextBoxBarcodeN2.Text = BarcodeN2
                            TextBoxBarcodeN2.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest2.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest2.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep1 = False
                            BarcodeStep2 = True

                            JigN1On()
                            JigN2Off()
                            JigN3On()
                            JigN4On()

                            TextBoxBarcode.Clear()
                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN2 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

                If BarcodeStep1 = False And BarcodeStep2 = True And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN4 = TextBoxBarcode.Text
                    ModelN4 = Microsoft.VisualBasic.Mid(BarcodeN4, 1, 2)

                    If Model = ModelN4 Then
                        Model_SelectModelN4 = True
                    Else
                        Model_SelectModelN4 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                    End If

                    If Model_SelectModelN4 = True Then
                        SelectModelN4 = True
                        TextBoxBarcodeN4.ReadOnly = True
                    Else
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                        SelectModelN4 = False
                    End If

                    If BarcodeN4 = BarcodeN2 Or BarcodeN4 = BarcodeN1 Then
                        BarcodeN4 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                    Else
                        If SelectModelN4 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N4()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN4 = True And SafetySensor = True Then
                            TextBoxBarcodeN4.Text = BarcodeN4
                            TextBoxBarcodeN4.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep2 = False

                            JigN1On()
                            JigN2On()
                            JigN3On()
                            JigN4Off()

                            TextBoxBarcode.Clear()
                            Barcode_Ready_Auto = True

                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN4 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

            ElseIf Condition1 = True And Condition2 = False And Condition3 = True And Condition4 = True Then
                '===== Nest 1, 3, 4 ====='
                Model_SelectModelN1 = False
                Model_SelectModelN3 = False
                Model_SelectModelN4 = False

                If BarcodeStep1 = False And BarcodeStep2 = False Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN1 = TextBoxBarcode.Text
                    ModelN1 = Microsoft.VisualBasic.Mid(BarcodeN1, 1, 2)

                    If Model = ModelN1 Then
                        Model_SelectModelN1 = True
                    Else
                        Model_SelectModelN1 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                    End If

                    If Model_SelectModelN1 = True Then
                        SelectModelN1 = True
                        TextBoxBarcodeN1.ReadOnly = True
                    Else
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                        SelectModelN1 = False
                    End If

                    If SelectModelN1 = True Then
                        If CheckBoxSendCIM.Checked = True Then
                            Write101N1()
                            Delay_Ms(200)
                            Read102()
                            Delay_Ms(50)
                        Else
                            If CIMRun = False Then
                                CIMRun = True
                                LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                            End If
                        End If
                    End If

                    CheckSafetySensor()

                    If CIMRun = True And SelectModelN1 = True And SafetySensor = True Then
                        TextBoxBarcodeN1.Text = BarcodeN1
                        TextBoxBarcodeN1.BackColor = Color.Cyan

                        For i = 0 To 40
                            For j = 1 To 5
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Value = ""
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                            Next
                        Next

                        BarcodeStep1 = True

                        JigN1Off()
                        JigN2On()
                        JigN3On()
                        JigN4On()

                        TextBoxBarcode.Clear()
                    ElseIf CIMRun = False Then
                        TextBoxInfo.Text = "CHECK CIM HISTORY!"
                        TextBoxBarcode.Clear()
                    ElseIf SelectModelN1 = False Then
                        TextBoxInfo.Text = "CHECK MODEL!"
                        TextBoxBarcode.Clear()
                    ElseIf SafetySensor = False Then
                        TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                        TextBoxBarcode.Clear()
                    End If
                End If

                If BarcodeStep1 = True And BarcodeStep2 = False And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN3 = TextBoxBarcode.Text
                    ModelN3 = Microsoft.VisualBasic.Mid(BarcodeN3, 1, 2)

                    If Model = ModelN3 Then
                        Model_SelectModelN3 = True
                    Else
                        Model_SelectModelN3 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                    End If

                    If Model_SelectModelN3 = True Then
                        SelectModelN3 = True
                        TextBoxBarcodeN3.ReadOnly = True
                    Else
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                        SelectModelN3 = False
                    End If

                    If BarcodeN3 = BarcodeN1 Then
                        BarcodeN3 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                    Else
                        If SelectModelN3 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N3()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN3 = True And SafetySensor = True Then
                            TextBoxBarcodeN3.Text = BarcodeN3
                            TextBoxBarcodeN3.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest3.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest3.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep1 = False
                            BarcodeStep2 = True

                            JigN1On()
                            JigN2On()
                            JigN3Off()
                            JigN4On()

                            TextBoxBarcode.Clear()
                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN3 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

                If BarcodeStep1 = False And BarcodeStep2 = True And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN4 = TextBoxBarcode.Text
                    ModelN4 = Microsoft.VisualBasic.Mid(BarcodeN4, 1, 2)

                    If Model = ModelN4 Then
                        Model_SelectModelN4 = True
                    Else
                        Model_SelectModelN4 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                    End If

                    If Model_SelectModelN4 = True Then
                        SelectModelN4 = True
                        TextBoxBarcodeN4.ReadOnly = True
                    Else
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                        SelectModelN4 = False
                    End If

                    If BarcodeN4 = BarcodeN3 Or BarcodeN4 = BarcodeN1 Then
                        BarcodeN4 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                    Else
                        If SelectModelN4 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N4()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN4 = True And SafetySensor = True Then
                            TextBoxBarcodeN4.Text = BarcodeN4
                            TextBoxBarcodeN4.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep2 = False

                            JigN1On()
                            JigN2On()
                            JigN3On()
                            JigN4Off()

                            TextBoxBarcode.Clear()
                            Barcode_Ready_Auto = True

                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN4 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

            ElseIf Condition1 = False And Condition2 = True And Condition3 = True And Condition4 = True Then
                '===== Nest 2, 3, 4 ====='
                Model_SelectModelN2 = False
                Model_SelectModelN3 = False
                Model_SelectModelN4 = False

                If BarcodeStep1 = False And BarcodeStep2 = False Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN2 = TextBoxBarcode.Text
                    ModelN2 = Microsoft.VisualBasic.Mid(BarcodeN2, 1, 2)

                    If Model = ModelN2 Then
                        Model_SelectModelN2 = True
                    Else
                        Model_SelectModelN2 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                    End If

                    If Model_SelectModelN2 = True Then
                        SelectModelN2 = True
                        TextBoxBarcodeN2.ReadOnly = True
                    Else
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                        SelectModelN2 = False
                    End If

                    If SelectModelN2 = True Then
                        If CheckBoxSendCIM.Checked = True Then
                            Write101N2()
                            Delay_Ms(200)
                            Read102()
                            Delay_Ms(50)
                        Else
                            If CIMRun = False Then
                                CIMRun = True
                                LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                            End If
                        End If
                    End If

                    CheckSafetySensor()

                    If CIMRun = True And SelectModelN2 = True And SafetySensor = True Then
                        TextBoxBarcodeN2.Text = BarcodeN2
                        TextBoxBarcodeN2.BackColor = Color.Cyan

                        For i = 0 To 40
                            For j = 1 To 5
                                DataGridViewProcessNest2.Rows.Item(i).Cells(j).Value = ""
                                DataGridViewProcessNest2.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                            Next
                        Next

                        BarcodeStep1 = True

                        JigN1On()
                        JigN2Off()
                        JigN3On()
                        JigN4On()

                        TextBoxBarcode.Clear()
                    ElseIf CIMRun = False Then
                        TextBoxInfo.Text = "CHECK CIM HISTORY!"
                        TextBoxBarcode.Clear()
                    ElseIf SelectModelN2 = False Then
                        TextBoxInfo.Text = "CHECK MODEL!"
                        TextBoxBarcode.Clear()
                    ElseIf SafetySensor = False Then
                        TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                        TextBoxBarcode.Clear()
                    End If
                End If

                If BarcodeStep1 = True And BarcodeStep2 = False And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN3 = TextBoxBarcode.Text
                    ModelN3 = Microsoft.VisualBasic.Mid(BarcodeN3, 1, 2)

                    If Model = ModelN3 Then
                        Model_SelectModelN3 = True
                    Else
                        Model_SelectModelN3 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                    End If

                    If Model_SelectModelN3 = True Then
                        SelectModelN3 = True
                        TextBoxBarcodeN3.ReadOnly = True
                    Else
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                        SelectModelN3 = False
                    End If

                    If BarcodeN3 = BarcodeN2 Then
                        BarcodeN3 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                    Else
                        If SelectModelN3 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N3()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN3 = True And SafetySensor = True Then
                            TextBoxBarcodeN3.Text = BarcodeN3
                            TextBoxBarcodeN3.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest3.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest3.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep1 = False
                            BarcodeStep2 = True

                            JigN1On()
                            JigN2On()
                            JigN3Off()
                            JigN4On()

                            TextBoxBarcode.Clear()
                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN3 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

                If BarcodeStep1 = False And BarcodeStep2 = True And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN4 = TextBoxBarcode.Text
                    ModelN4 = Microsoft.VisualBasic.Mid(BarcodeN4, 1, 2)

                    If Model = ModelN4 Then
                        Model_SelectModelN4 = True
                    Else
                        Model_SelectModelN4 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                    End If

                    If Model_SelectModelN4 = True Then
                        SelectModelN4 = True
                        TextBoxBarcodeN4.ReadOnly = True
                    Else
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                        SelectModelN4 = False
                    End If

                    If BarcodeN4 = BarcodeN3 Or BarcodeN4 = BarcodeN2 Then
                        BarcodeN4 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                    Else
                        If SelectModelN4 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N4()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN4 = True And SafetySensor = True Then
                            TextBoxBarcodeN4.Text = BarcodeN4
                            TextBoxBarcodeN4.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep2 = False

                            JigN1On()
                            JigN2On()
                            JigN3On()
                            JigN4Off()

                            TextBoxBarcode.Clear()
                            Barcode_Ready_Auto = True

                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN4 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

            ElseIf Condition1 = True And Condition2 = True And Condition3 = False And Condition4 = False Then
                '===== Nest 1, 2 ====='
                Model_SelectModelN1 = False
                Model_SelectModelN2 = False

                If BarcodeStep1 = False Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN1 = TextBoxBarcode.Text
                    ModelN1 = Microsoft.VisualBasic.Mid(BarcodeN1, 1, 2)

                    If Model = ModelN1 Then
                        Model_SelectModelN1 = True
                    Else
                        Model_SelectModelN1 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                    End If

                    If Model_SelectModelN1 = True Then
                        SelectModelN1 = True
                        TextBoxBarcodeN1.ReadOnly = True
                    Else
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                        SelectModelN1 = False
                    End If

                    If SelectModelN1 = True Then
                        If CheckBoxSendCIM.Checked = True Then
                            Write101N1()
                            Delay_Ms(200)
                            Read102()
                            Delay_Ms(50)
                        Else
                            If CIMRun = False Then
                                CIMRun = True
                                LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                            End If
                        End If
                    End If

                    CheckSafetySensor()

                    If CIMRun = True And SelectModelN1 = True And SafetySensor = True Then
                        TextBoxBarcodeN1.Text = BarcodeN1
                        TextBoxBarcodeN1.BackColor = Color.Cyan

                        For i = 0 To 40
                            For j = 1 To 5
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Value = ""
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                            Next
                        Next

                        BarcodeStep1 = True
                        RequirePostBarcodeStart = True
                        StartButtonReleasedAfterBarcode = False

                        ' Pergerakan jig ditunda sampai kedua barcode Nest 1 dan 2 valid.
                        ' JigN1Off()
                        ' JigN2On()
                        ' JigN3On()
                        ' JigN4On()

                        TextBoxBarcode.Clear()
                    ElseIf CIMRun = False Then
                        TextBoxInfo.Text = "CHECK CIM HISTORY!"
                        TextBoxBarcode.Clear()
                    ElseIf SelectModelN1 = False Then
                        TextBoxInfo.Text = "CHECK MODEL!"
                        TextBoxBarcode.Clear()
                    ElseIf SafetySensor = False Then
                        TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                        TextBoxBarcode.Clear()
                    End If
                End If

                If BarcodeStep1 = True And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN2 = TextBoxBarcode.Text
                    ModelN2 = Microsoft.VisualBasic.Mid(BarcodeN2, 1, 2)

                    If Model = ModelN2 Then
                        Model_SelectModelN2 = True
                    Else
                        Model_SelectModelN2 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                    End If

                    If Model_SelectModelN2 = True Then
                        SelectModelN2 = True
                        TextBoxBarcodeN2.ReadOnly = True
                    Else
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                        SelectModelN2 = False
                    End If

                    If BarcodeN2 = BarcodeN1 Then
                        BarcodeN2 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                    Else
                        If SelectModelN2 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N2()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN2 = True And SafetySensor = True Then
                            TextBoxBarcodeN2.Text = BarcodeN2
                            TextBoxBarcodeN2.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest2.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest2.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep1 = False

                            ' Perintah pergerakan per-scan dipertahankan sebagai komentar; semua jig ON setelah kedua barcode valid.
                            ' JigN1On()
                            ' JigN2Off()
                            ' JigN3On()
                            ' JigN4On()

                            TextBoxBarcode.Clear()
                            Barcode_Ready_Auto = True
                            JigN1On()
                            JigN2On()
                            JigN3On()
                            JigN4On()

                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN2 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

            ElseIf Condition1 = True And Condition2 = False And Condition3 = True And Condition4 = False Then
                '===== Nest 1, 3 ====='
                Model_SelectModelN1 = False
                Model_SelectModelN3 = False

                If BarcodeStep1 = False Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN1 = TextBoxBarcode.Text
                    ModelN1 = Microsoft.VisualBasic.Mid(BarcodeN1, 1, 2)

                    If Model = ModelN1 Then
                        Model_SelectModelN1 = True
                    Else
                        Model_SelectModelN1 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                    End If

                    If Model_SelectModelN1 = True Then
                        SelectModelN1 = True
                        TextBoxBarcodeN1.ReadOnly = True
                    Else
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                        SelectModelN1 = False
                    End If

                    If SelectModelN1 = True Then
                        If CheckBoxSendCIM.Checked = True Then
                            Write101N1()
                            Delay_Ms(200)
                            Read102()
                            Delay_Ms(50)
                        Else
                            If CIMRun = False Then
                                CIMRun = True
                                LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                            End If
                        End If
                    End If

                    CheckSafetySensor()

                    If CIMRun = True And SelectModelN1 = True And SafetySensor = True Then
                        TextBoxBarcodeN1.Text = BarcodeN1
                        TextBoxBarcodeN1.BackColor = Color.Cyan

                        For i = 0 To 40
                            For j = 1 To 5
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Value = ""
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                            Next
                        Next

                        BarcodeStep1 = True

                        JigN1Off()
                        JigN2On()
                        JigN3On()
                        JigN4On()

                        TextBoxBarcode.Clear()
                    ElseIf CIMRun = False Then
                        TextBoxInfo.Text = "CHECK CIM HISTORY!"
                        TextBoxBarcode.Clear()
                    ElseIf SelectModelN1 = False Then
                        TextBoxInfo.Text = "CHECK MODEL!"
                        TextBoxBarcode.Clear()
                    ElseIf SafetySensor = False Then
                        TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                        TextBoxBarcode.Clear()
                    End If
                End If

                If BarcodeStep1 = True And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN3 = TextBoxBarcode.Text
                    ModelN3 = Microsoft.VisualBasic.Mid(BarcodeN3, 1, 2)

                    If Model = ModelN3 Then
                        Model_SelectModelN3 = True
                    Else
                        Model_SelectModelN3 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                    End If

                    If Model_SelectModelN3 = True Then
                        SelectModelN3 = True
                        TextBoxBarcodeN3.ReadOnly = True
                    Else
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                        SelectModelN3 = False
                    End If

                    If BarcodeN3 = BarcodeN1 Then
                        BarcodeN3 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                    Else
                        If SelectModelN3 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N3()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN3 = True And SafetySensor = True Then
                            TextBoxBarcodeN3.Text = BarcodeN3
                            TextBoxBarcodeN3.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest3.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest3.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep1 = False

                            JigN1On()
                            JigN2On()
                            JigN3Off()
                            JigN4On()

                            TextBoxBarcode.Clear()
                            Barcode_Ready_Auto = True

                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN3 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

            ElseIf Condition1 = True And Condition2 = False And Condition3 = False And Condition4 = True Then
                '===== Nest 1, 4 ====='

                If BarcodeStep1 = False Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN1 = TextBoxBarcode.Text
                    ModelN1 = Microsoft.VisualBasic.Mid(BarcodeN1, 1, 2)

                    If Model = ModelN1 Then
                        Model_SelectModelN1 = True
                    Else
                        Model_SelectModelN1 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                    End If

                    If Model_SelectModelN1 = True Then
                        SelectModelN1 = True
                        TextBoxBarcodeN1.ReadOnly = True
                    Else
                        TextBoxBarcodeN1.Clear()
                        TextBoxBarcodeN1.BackColor = Color.White
                        TextBoxBarcodeN1.ReadOnly = False
                        SelectModelN1 = False
                    End If

                    If SelectModelN1 = True Then
                        If CheckBoxSendCIM.Checked = True Then
                            Write101N1()
                            Delay_Ms(200)
                            Read102()
                            Delay_Ms(50)
                        Else
                            If CIMRun = False Then
                                CIMRun = True
                                LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                            End If
                        End If
                    End If

                    CheckSafetySensor()

                    If CIMRun = True And SelectModelN1 = True And SafetySensor = True Then
                        TextBoxBarcodeN1.Text = BarcodeN1
                        TextBoxBarcodeN1.BackColor = Color.Cyan

                        For i = 0 To 40
                            For j = 1 To 5
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Value = ""
                                DataGridViewProcessNest1.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                            Next
                        Next

                        BarcodeStep1 = True

                        JigN1Off()
                        JigN2On()
                        JigN3On()
                        JigN4On()

                        TextBoxBarcode.Clear()
                    ElseIf CIMRun = False Then
                        TextBoxInfo.Text = "CHECK CIM HISTORY!"
                        TextBoxBarcode.Clear()
                    ElseIf SelectModelN1 = False Then
                        TextBoxInfo.Text = "CHECK MODEL!"
                        TextBoxBarcode.Clear()
                    ElseIf SafetySensor = False Then
                        TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                        TextBoxBarcode.Clear()
                    End If
                End If

                If BarcodeStep1 = True And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN4 = TextBoxBarcode.Text
                    ModelN4 = Microsoft.VisualBasic.Mid(BarcodeN4, 1, 2)

                    If Model = ModelN4 Then
                        Model_SelectModelN4 = True
                    Else
                        Model_SelectModelN4 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                    End If

                    If Model_SelectModelN4 = True Then
                        SelectModelN4 = True
                        TextBoxBarcodeN4.ReadOnly = True
                    Else
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                        SelectModelN4 = False
                    End If

                    If BarcodeN4 = BarcodeN1 Then
                        BarcodeN4 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                    Else
                        If SelectModelN4 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N4()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN4 = True And SafetySensor = True Then
                            TextBoxBarcodeN4.Text = BarcodeN4
                            TextBoxBarcodeN4.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep1 = False

                            JigN1On()
                            JigN2On()
                            JigN3On()
                            JigN4Off()

                            TextBoxBarcode.Clear()
                            Barcode_Ready_Auto = True

                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN4 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

            ElseIf Condition1 = False And Condition2 = True And Condition3 = True And Condition4 = False Then
                '===== Nest 2, 3 ====='
                Model_SelectModelN2 = False
                Model_SelectModelN3 = False

                If BarcodeStep1 = False Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN2 = TextBoxBarcode.Text
                    ModelN2 = Microsoft.VisualBasic.Mid(BarcodeN2, 1, 2)

                    If Model = ModelN2 Then
                        Model_SelectModelN2 = True
                    Else
                        Model_SelectModelN2 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                    End If

                    If Model_SelectModelN2 = True Then
                        SelectModelN2 = True
                        TextBoxBarcodeN2.ReadOnly = True
                    Else
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                        SelectModelN2 = False
                    End If

                    If SelectModelN2 = True Then
                        If CheckBoxSendCIM.Checked = True Then
                            Write101N2()
                            Delay_Ms(200)
                            Read102()
                            Delay_Ms(50)
                        Else
                            If CIMRun = False Then
                                CIMRun = True
                                LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                            End If
                        End If
                    End If

                    CheckSafetySensor()

                    If CIMRun = True And SelectModelN2 = True And SafetySensor = True Then
                        TextBoxBarcodeN2.Text = BarcodeN2
                        TextBoxBarcodeN2.BackColor = Color.Cyan

                        For i = 0 To 40
                            For j = 1 To 5
                                DataGridViewProcessNest2.Rows.Item(i).Cells(j).Value = ""
                                DataGridViewProcessNest2.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                            Next
                        Next

                        BarcodeStep1 = True

                        JigN1On()
                        JigN2Off()
                        JigN3On()
                        JigN4On()

                        TextBoxBarcode.Clear()
                    ElseIf CIMRun = False Then
                        TextBoxInfo.Text = "CHECK CIM HISTORY!"
                        TextBoxBarcode.Clear()
                    ElseIf SelectModelN2 = False Then
                        TextBoxInfo.Text = "CHECK MODEL!"
                        TextBoxBarcode.Clear()
                    ElseIf SafetySensor = False Then
                        TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                        TextBoxBarcode.Clear()
                    End If
                End If

                If BarcodeStep1 = True And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN3 = TextBoxBarcode.Text
                    ModelN3 = Microsoft.VisualBasic.Mid(BarcodeN3, 1, 2)

                    If Model = ModelN3 Then
                        Model_SelectModelN3 = True
                    Else
                        Model_SelectModelN3 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                    End If

                    If Model_SelectModelN3 = True Then
                        SelectModelN3 = True
                        TextBoxBarcodeN3.ReadOnly = True
                    Else
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                        SelectModelN3 = False
                    End If

                    If BarcodeN3 = BarcodeN2 Then
                        BarcodeN3 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                    Else
                        If SelectModelN3 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N3()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN3 = True And SafetySensor = True Then
                            TextBoxBarcodeN3.Text = BarcodeN3
                            TextBoxBarcodeN3.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest3.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest3.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep1 = False

                            JigN1On()
                            JigN2On()
                            JigN3Off()
                            JigN4On()

                            TextBoxBarcode.Clear()
                            Barcode_Ready_Auto = True

                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN3 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

            ElseIf Condition1 = False And Condition2 = True And Condition3 = False And Condition4 = True Then
                '===== Nest 2, 4 ====='
                Model_SelectModelN2 = False
                Model_SelectModelN4 = False

                If BarcodeStep1 = False Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN2 = TextBoxBarcode.Text
                    ModelN2 = Microsoft.VisualBasic.Mid(BarcodeN2, 1, 2)

                    If Model = ModelN2 Then
                        Model_SelectModelN2 = True
                    Else
                        Model_SelectModelN2 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                    End If

                    If Model_SelectModelN2 = True Then
                        SelectModelN2 = True
                        TextBoxBarcodeN2.ReadOnly = True
                    Else
                        TextBoxBarcodeN2.Clear()
                        TextBoxBarcodeN2.BackColor = Color.White
                        TextBoxBarcodeN2.ReadOnly = False
                        SelectModelN2 = False
                    End If

                    If SelectModelN2 = True Then
                        If CheckBoxSendCIM.Checked = True Then
                            Write101N2()
                            Delay_Ms(200)
                            Read102()
                            Delay_Ms(50)
                        Else
                            If CIMRun = False Then
                                CIMRun = True
                                LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                            End If
                        End If
                    End If

                    CheckSafetySensor()

                    If CIMRun = True And SelectModelN2 = True And SafetySensor = True Then
                        TextBoxBarcodeN2.Text = BarcodeN2
                        TextBoxBarcodeN2.BackColor = Color.Cyan

                        For i = 0 To 40
                            For j = 1 To 5
                                DataGridViewProcessNest2.Rows.Item(i).Cells(j).Value = ""
                                DataGridViewProcessNest2.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                            Next
                        Next

                        BarcodeStep1 = True

                        JigN1On()
                        JigN2Off()
                        JigN3On()
                        JigN4On()

                        TextBoxBarcode.Clear()
                    ElseIf CIMRun = False Then
                        TextBoxInfo.Text = "CHECK CIM HISTORY!"
                        TextBoxBarcode.Clear()
                    ElseIf SelectModelN2 = False Then
                        TextBoxInfo.Text = "CHECK MODEL!"
                        TextBoxBarcode.Clear()
                    ElseIf SafetySensor = False Then
                        TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                        TextBoxBarcode.Clear()
                    End If
                End If

                If BarcodeStep1 = True And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN4 = TextBoxBarcode.Text
                    ModelN4 = Microsoft.VisualBasic.Mid(BarcodeN4, 1, 2)

                    If Model = ModelN4 Then
                        Model_SelectModelN4 = True
                    Else
                        Model_SelectModelN4 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                    End If

                    If Model_SelectModelN4 = True Then
                        SelectModelN4 = True
                        TextBoxBarcodeN4.ReadOnly = True
                    Else
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                        SelectModelN4 = False
                    End If

                    If BarcodeN4 = BarcodeN2 Then
                        BarcodeN4 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                    Else
                        If SelectModelN4 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N4()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN4 = True And SafetySensor = True Then
                            TextBoxBarcodeN4.Text = BarcodeN4
                            TextBoxBarcodeN4.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep1 = False

                            JigN1On()
                            JigN2On()
                            JigN3On()
                            JigN4Off()

                            TextBoxBarcode.Clear()
                            Barcode_Ready_Auto = True

                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN4 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

            ElseIf Condition1 = False And Condition2 = False And Condition3 = True And Condition4 = True Then
                '===== Nest 3, 4 ====='
                Model_SelectModelN3 = False
                Model_SelectModelN4 = False

                If BarcodeStep1 = False Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN3 = TextBoxBarcode.Text
                    ModelN3 = Microsoft.VisualBasic.Mid(BarcodeN3, 1, 2)

                    If Model = ModelN3 Then
                        Model_SelectModelN3 = True
                    Else
                        Model_SelectModelN3 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                    End If

                    If Model_SelectModelN3 = True Then
                        SelectModelN3 = True
                        TextBoxBarcodeN3.ReadOnly = True
                    Else
                        TextBoxBarcodeN3.Clear()
                        TextBoxBarcodeN3.BackColor = Color.White
                        TextBoxBarcodeN3.ReadOnly = False
                        SelectModelN3 = False
                    End If

                    If SelectModelN3 = True Then
                        If CheckBoxSendCIM.Checked = True Then
                            Write101N3()
                            Delay_Ms(200)
                            Read102()
                            Delay_Ms(50)
                        Else
                            If CIMRun = False Then
                                CIMRun = True
                                LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                            End If
                        End If
                    End If

                    CheckSafetySensor()

                    If CIMRun = True And SelectModelN3 = True And SafetySensor = True Then
                        TextBoxBarcodeN3.Text = BarcodeN3
                        TextBoxBarcodeN3.BackColor = Color.Cyan

                        For i = 0 To 40
                            For j = 1 To 5
                                DataGridViewProcessNest3.Rows.Item(i).Cells(j).Value = ""
                                DataGridViewProcessNest3.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                            Next
                        Next

                        BarcodeStep1 = True

                        JigN1On()
                        JigN2On()
                        JigN3Off()
                        JigN4On()

                        TextBoxBarcode.Clear()
                    ElseIf CIMRun = False Then
                        TextBoxInfo.Text = "CHECK CIM HISTORY!"
                        TextBoxBarcode.Clear()
                    ElseIf SelectModelN3 = False Then
                        TextBoxInfo.Text = "CHECK MODEL!"
                        TextBoxBarcode.Clear()
                    ElseIf SafetySensor = False Then
                        TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                        TextBoxBarcode.Clear()
                    End If
                End If

                If BarcodeStep1 = True And Len(TextBoxBarcode.Text) = 16 Then
                    System.Windows.Forms.Application.DoEvents()
                    BarcodeN4 = TextBoxBarcode.Text
                    ModelN4 = Microsoft.VisualBasic.Mid(BarcodeN4, 1, 2)

                    If Model = ModelN4 Then
                        Model_SelectModelN4 = True
                    Else
                        Model_SelectModelN4 = False
                        TextBoxInfo.Text = "WRONG BARCODE"
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                    End If

                    If Model_SelectModelN4 = True Then
                        SelectModelN4 = True
                        TextBoxBarcodeN4.ReadOnly = True
                    Else
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                        TextBoxBarcodeN4.ReadOnly = False
                        SelectModelN4 = False
                    End If

                    If BarcodeN4 = BarcodeN3 Then
                        BarcodeN4 = ""
                        TextBoxInfo.Text = "RESCAN PWBA!"
                        Delay_Ms(200)
                        TextBoxBarcode.Clear()
                        TextBoxBarcodeN4.Clear()
                        TextBoxBarcodeN4.BackColor = Color.White
                    Else
                        If SelectModelN4 = True Then
                            TextBoxInfo.Text = ""
                            If CheckBoxSendCIM.Checked = True Then
                                Write101N4()
                                Delay_Ms(200)
                                Read102()
                                Delay_Ms(50)
                            Else
                                If CIMRun = False Then
                                    CIMRun = True
                                    LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                                    TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                                End If
                            End If
                        End If

                        CheckSafetySensor()

                        If CIMRun = True And SelectModelN4 = True And SafetySensor = True Then
                            TextBoxBarcodeN4.Text = BarcodeN4
                            TextBoxBarcodeN4.BackColor = Color.Cyan

                            For i = 0 To 40
                                For j = 1 To 5
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Value = ""
                                    DataGridViewProcessNest4.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                                Next
                            Next

                            BarcodeStep1 = False

                            JigN1On()
                            JigN2On()
                            JigN3On()
                            JigN4Off()

                            TextBoxBarcode.Clear()
                            Barcode_Ready_Auto = True

                        ElseIf CIMRun = False Then
                            TextBoxInfo.Text = "CHECK CIM HISTORY!"
                            TextBoxBarcode.Clear()
                        ElseIf SelectModelN4 = False Then
                            TextBoxInfo.Text = "CHECK MODEL!"
                            TextBoxBarcode.Clear()
                        ElseIf SafetySensor = False Then
                            TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                            TextBoxBarcode.Clear()
                        End If
                    End If
                End If

            ElseIf Condition1 = True And Condition2 = False And Condition3 = False And Condition4 = False Then
                '===== Nest 1 ====='
                System.Windows.Forms.Application.DoEvents()
                BarcodeN1 = TextBoxBarcode.Text
                ModelN1 = Microsoft.VisualBasic.Mid(BarcodeN1, 1, 2)

                If Model = ModelN1 Then
                    Model_SelectModelN1 = True
                Else
                    Model_SelectModelN1 = False
                    TextBoxInfo.Text = "WRONG BARCODE"
                    TextBoxBarcodeN1.Clear()
                    TextBoxBarcodeN1.BackColor = Color.White
                    TextBoxBarcodeN1.ReadOnly = False
                End If

                If Model_SelectModelN1 = True Then
                    SelectModelN1 = True
                    TextBoxBarcodeN1.ReadOnly = True
                Else
                    TextBoxBarcodeN1.Clear()
                    TextBoxBarcodeN1.BackColor = Color.White
                    TextBoxBarcodeN1.ReadOnly = False
                    SelectModelN1 = False
                End If

                If SelectModelN1 = True Then
                    If CheckBoxSendCIM.Checked = True Then
                        Write101N1()
                        Delay_Ms(200)
                        Read102()
                        Delay_Ms(50)
                    Else
                        If CIMRun = False Then
                            CIMRun = True
                            LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                            TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                        End If
                    End If
                End If

                CheckSafetySensor()

                If CIMRun = True And SelectModelN1 = True And SafetySensor = True Then
                    TextBoxBarcodeN1.Text = BarcodeN1
                    TextBoxBarcodeN1.BackColor = Color.Cyan

                    For i = 0 To 40
                        For j = 1 To 5
                            DataGridViewProcessNest1.Rows.Item(i).Cells(j).Value = ""
                            DataGridViewProcessNest1.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                        Next
                    Next

                    JigN1Off()
                    JigN2On()
                    JigN3On()
                    JigN4On()

                    TextBoxBarcode.Clear()
                    Barcode_Ready_Auto = True

                ElseIf CIMRun = False Then
                    TextBoxInfo.Text = "CHECK CIM HISTORY!"
                    TextBoxBarcode.Clear()
                ElseIf SelectModelN1 = False Then
                    TextBoxInfo.Text = "CHECK MODEL!"
                    TextBoxBarcode.Clear()
                ElseIf SafetySensor = False Then
                    TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                    TextBoxBarcode.Clear()
                End If

            ElseIf Condition1 = False And Condition2 = True And Condition3 = False And Condition4 = False Then
                '===== Nest 2 ====='
                System.Windows.Forms.Application.DoEvents()
                BarcodeN2 = TextBoxBarcode.Text
                ModelN2 = Microsoft.VisualBasic.Mid(BarcodeN2, 1, 2)

                If Model = ModelN2 Then
                    Model_SelectModelN2 = True
                Else
                    Model_SelectModelN2 = False
                    TextBoxInfo.Text = "WRONG BARCODE"
                    TextBoxBarcodeN2.Clear()
                    TextBoxBarcodeN2.BackColor = Color.White
                    TextBoxBarcodeN2.ReadOnly = False
                End If

                If Model_SelectModelN2 = True Then
                    SelectModelN2 = True
                    TextBoxBarcodeN2.ReadOnly = True
                Else
                    TextBoxBarcodeN2.Clear()
                    TextBoxBarcodeN2.BackColor = Color.White
                    TextBoxBarcodeN2.ReadOnly = False
                    SelectModelN2 = False
                End If

                If SelectModelN2 = True Then
                    If CheckBoxSendCIM.Checked = True Then
                        Write101N2()
                        Delay_Ms(200)
                        Read102()
                        Delay_Ms(50)
                    Else
                        If CIMRun = False Then
                            CIMRun = True
                            LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                            TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                        End If
                    End If
                End If

                CheckSafetySensor()

                If CIMRun = True And SelectModelN2 = True And SafetySensor = True Then
                    TextBoxBarcodeN2.Text = BarcodeN2
                    TextBoxBarcodeN2.BackColor = Color.Cyan

                    For i = 0 To 40
                        For j = 1 To 5
                            DataGridViewProcessNest2.Rows.Item(i).Cells(j).Value = ""
                            DataGridViewProcessNest2.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                        Next
                    Next

                    JigN1On()
                    JigN2Off()
                    JigN3On()
                    JigN4On()

                    TextBoxBarcode.Clear()
                    Barcode_Ready_Auto = True

                ElseIf CIMRun = False Then
                    TextBoxInfo.Text = "CHECK CIM HISTORY!"
                    TextBoxBarcode.Clear()
                ElseIf SelectModelN2 = False Then
                    TextBoxInfo.Text = "CHECK MODEL!"
                    TextBoxBarcode.Clear()
                ElseIf SafetySensor = False Then
                    TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                    TextBoxBarcode.Clear()
                End If

            ElseIf Condition1 = False And Condition2 = False And Condition3 = True And Condition4 = False Then
                '===== Nest 3 ====='
                System.Windows.Forms.Application.DoEvents()
                BarcodeN3 = TextBoxBarcode.Text
                ModelN3 = Microsoft.VisualBasic.Mid(BarcodeN3, 1, 2)

                If Model = ModelN3 Then
                    Model_SelectModelN3 = True
                Else
                    Model_SelectModelN3 = False
                    TextBoxInfo.Text = "WRONG BARCODE"
                    TextBoxBarcodeN3.Clear()
                    TextBoxBarcodeN3.BackColor = Color.White
                    TextBoxBarcodeN3.ReadOnly = False
                End If

                If Model_SelectModelN3 = True Then
                    SelectModelN3 = True
                    TextBoxBarcodeN3.ReadOnly = True
                Else
                    TextBoxBarcodeN3.Clear()
                    TextBoxBarcodeN3.BackColor = Color.White
                    TextBoxBarcodeN3.ReadOnly = False
                    SelectModelN3 = False
                End If

                If SelectModelN3 = True Then
                    If CheckBoxSendCIM.Checked = True Then
                        Write101N3()
                        Delay_Ms(200)
                        Read102()
                        Delay_Ms(50)
                    Else
                        If CIMRun = False Then
                            CIMRun = True
                            LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                            TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                        End If
                    End If
                End If

                CheckSafetySensor()

                If CIMRun = True And SelectModelN3 = True And SafetySensor = True Then
                    TextBoxBarcodeN3.Text = BarcodeN3
                    TextBoxBarcodeN3.BackColor = Color.Cyan

                    For i = 0 To 40
                        For j = 1 To 5
                            DataGridViewProcessNest3.Rows.Item(i).Cells(j).Value = ""
                            DataGridViewProcessNest3.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                        Next
                    Next

                    JigN1On()
                    JigN2On()
                    JigN3Off()
                    JigN4On()

                    TextBoxBarcode.Clear()
                    Barcode_Ready_Auto = True

                ElseIf CIMRun = False Then
                    TextBoxInfo.Text = "CHECK CIM HISTORY!"
                    TextBoxBarcode.Clear()
                ElseIf SelectModelN3 = False Then
                    TextBoxInfo.Text = "CHECK MODEL!"
                    TextBoxBarcode.Clear()
                ElseIf SafetySensor = False Then
                    TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                    TextBoxBarcode.Clear()
                End If

            ElseIf Condition1 = False And Condition2 = False And Condition3 = False And Condition4 = True Then
                '===== Nest 4 ====='
                System.Windows.Forms.Application.DoEvents()
                BarcodeN4 = TextBoxBarcode.Text
                ModelN4 = Microsoft.VisualBasic.Mid(BarcodeN4, 1, 2)

                If Model = ModelN4 Then
                    Model_SelectModelN4 = True
                Else
                    Model_SelectModelN4 = False
                    TextBoxInfo.Text = "WRONG BARCODE"
                    TextBoxBarcodeN4.Clear()
                    TextBoxBarcodeN4.BackColor = Color.White
                    TextBoxBarcodeN4.ReadOnly = False
                End If

                If Model_SelectModelN4 = True Then
                    SelectModelN4 = True
                    TextBoxBarcodeN4.ReadOnly = True
                Else
                    TextBoxBarcodeN4.Clear()
                    TextBoxBarcodeN4.BackColor = Color.White
                    TextBoxBarcodeN4.ReadOnly = False
                    SelectModelN4 = False
                End If

                If SelectModelN4 = True Then
                    If CheckBoxSendCIM.Checked = True Then
                        Write101N4()
                        Delay_Ms(200)
                        Read102()
                        Delay_Ms(50)
                    Else
                        If CIMRun = False Then
                            CIMRun = True
                            LabelInfoCIM.Text = "RUNNING WITHOUT CIM"
                            TextBoxInfo.Text = "RUNNING WITHOUT CIM"
                        End If
                    End If
                End If

                CheckSafetySensor()

                If CIMRun = True And SelectModelN4 = True And SafetySensor = True Then
                    TextBoxBarcodeN4.Text = BarcodeN4
                    TextBoxBarcodeN4.BackColor = Color.Cyan

                    For i = 0 To 40
                        For j = 1 To 5
                            DataGridViewProcessNest4.Rows.Item(i).Cells(j).Value = ""
                            DataGridViewProcessNest4.Rows.Item(i).Cells(j).Style.BackColor = Color.White
                        Next
                    Next

                    JigN1On()
                    JigN2On()
                    JigN3On()
                    JigN4Off()

                    TextBoxBarcode.Clear()
                    Barcode_Ready_Auto = True

                ElseIf CIMRun = False Then
                    TextBoxInfo.Text = "CHECK CIM HISTORY!"
                    TextBoxBarcode.Clear()
                ElseIf SelectModelN4 = False Then
                    TextBoxInfo.Text = "CHECK MODEL!"
                    TextBoxBarcode.Clear()
                ElseIf SafetySensor = False Then
                    TextBoxInfo.Text = "CHECK SAFETY SENSOR!"
                    TextBoxBarcode.Clear()
                End If
            End If

        ElseIf PushButtonEmergency = False Then
            TextBoxInfo.Text = "RELEASE EMERGENCY!"
            TextBoxBarcode.Clear()
        End If

        CheckSafetySensor()

        ' Untuk Nest 1/2, wajib lepas kedua tombol setelah barcode lengkap lalu tekan kembali.
        If RequirePostBarcodeStart = True And Barcode_Ready_Auto = True And PushButtonLeft = False And PushButtonRight = False And PushButtonEmergency = True Then
            StartButtonReleasedAfterBarcode = True
        End If

        If AllProcess = True And ButtonReady = True And SafetySensor = True And (RequirePostBarcodeStart = False Or StartButtonReleasedAfterBarcode = True) Then
            If Barcode_Ready_Auto = True Then
                If CheckBoxSendCIM.Checked = False Then
                    MessageBox.Show("Running without CIM?", "FCT K2SA-K2SG-K3MF", MessageBoxButtons.OK, MessageBoxIcon.Question)
                End If

                Timer2.Enabled = True
                Timer1.Enabled = True
                CaseElseCondition = False
                StepTest = 1
                Me.Stopwatch.Reset()
                LabelMachineTime.Text = "00:00:00:000"
                Timer3.Start()
                Me.Stopwatch.Start()
                Barcode_Ready_Auto = False
                RequirePostBarcodeStart = False
                StartButtonReleasedAfterBarcode = False
                TextBoxInfo.Text = ""

            ElseIf SafetySensor = False Then
                TextBoxInfo.Text = "CHECK CIM HISTORY, MODEL, OR SAFETY SENSOR!"
            End If
        End If

        '##### EMERGENCY #####'

        If CaseElseCondition = True Then
            If (RunningNest1 = True And ProcessN1 = False) Or
                (RunningNest2 = True And ProcessN2 = False) Or
                (RunningNest3 = True And ProcessN3 = False) Or
                (RunningNest4 = True And ProcessN4 = False) Then

                Do Until (ValPort3CL And &H4) <> 0
                    Rd_DiPort()
                    AllProcess = True
                    ProcessN1 = True
                    ProcessN2 = True
                    ProcessN3 = True
                    ProcessN4 = True
                    BarcodeN1 = ""
                    BarcodeN2 = ""
                    BarcodeN3 = ""
                    BarcodeN4 = ""
                    TextBoxBarcode.Text = ""
                    TextBoxBarcode.BackColor = Color.White
                    System.Windows.Forms.Application.DoEvents()
                Loop

                System.Windows.Forms.Application.DoEvents()
                AllProcess = True
                StepTest = 0
                StepProcess = 0
                JigN1Off()
                Delay_Ms(200)
                JigN2Off()
                Delay_Ms(200)
                JigN3Off()
                Delay_Ms(200)
                JigN4Off()
                Delay_Ms(200)
                ClearPort()
                ResetIO()
                TextBoxBarcodeN1.Clear()
                TextBoxBarcodeN2.Clear()
                TextBoxBarcodeN3.Clear()
                TextBoxBarcodeN4.Clear()
            ElseIf PushButtonEmergency = False Then
                JigN1Off()
                Delay_Ms(200)
                JigN2Off()
                Delay_Ms(200)
                JigN3Off()
                Delay_Ms(200)
                JigN4Off()
                Delay_Ms(200)
                ClearPort()
                ResetIO()
                BarcodeN1 = ""
                BarcodeN2 = ""
                BarcodeN3 = ""
                BarcodeN4 = ""
                TextBoxBarcodeN1.Clear()
                TextBoxBarcodeN2.Clear()
                TextBoxBarcodeN3.Clear()
                TextBoxBarcodeN4.Clear()
                LabelProses.Text = "Emergency Pressed"
                LabelProses.BackColor = Color.IndianRed
                StepTest = 123
                CaseElseCondition = True
                Timer2.Enabled = True
            End If
        End If

        If CaseElseCondition = False And PushButtonEmergency = False Then
            JigN1Off()
            Delay_Ms(200)
            JigN2Off()
            Delay_Ms(200)
            JigN3Off()
            Delay_Ms(200)
            JigN4Off()
            Delay_Ms(200)
            ClearPort()
            ResetIO()
            TextBoxBarcode.Text = "Emergency Pressed"
            LabelProses.Text = "Emergency Pressed"
            LabelProses.BackColor = Color.IndianRed
            StepTest = 123
            CaseElseCondition = True
            Timer2.Enabled = True
        End If

        Interrupt = True
    End Sub

    Private Sub SerialPWBANest2_OnComm(sender As Object, e As EventArgs) Handles SerialPWBANest2.OnComm

    End Sub

    Private Sub DataGridViewProcessNest4_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewProcessNest4.CellContentClick

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Timer2.Enabled = False
        InputCheck()
        Timer2.Enabled = True
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        'Dim Elapsed As TimeSpan = Me.Stopwatch.Elapsed
        'LabelMachineTime.Text = String.Format("{0:00}:{1:00}:{2:00}:{3:00}",
        'Math.Floor(Elapsed.TotalHours),
        'Elapsed.Minutes, Elapsed.Seconds,
        'Elapsed.Milliseconds)
    End Sub

    Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick
        Rd_DiPort()

        TextBoxManualCheckInput.Text = Convert.ToString(ValPort3A, 2).PadLeft(8, "0"c) & vbCrLf & Convert.ToString(ValPort3B, 2).PadLeft(8, "0"c) & vbCrLf & Convert.ToString(ValPort3CL, 2).PadLeft(4, "0"c) & vbCrLf & Convert.ToString(ValPort3CH, 2).PadLeft(4, "0"c)
    End Sub

    Public Sub Delay_Ms(ByVal TimeOut As Long)
        Dim TimeCnt As Long
        TimeCnt = GetTickCount()
        Do While ((GetTickCount() - TimeCnt) < TimeOut)
            TimeCnt = TimeCnt
        Loop
    End Sub

    '===== POWER SUPPLY TEKTRONIX =====

    Private Sub ButtonPSU13V_Click(sender As Object, e As EventArgs) Handles ButtonPSU13V.Click
        PSU13V()
    End Sub

    Private Sub ButtonPSU15V_Click(sender As Object, e As EventArgs) Handles ButtonPSU15V.Click
        PSU15V()
    End Sub

    Public Sub PSU13V()
        If myPWS.Initialized = False Then
            myPWS.Initialize("USB0::0x0699::0x0390::C010255::0::INSTR", True, False)
            myPWSOutPut = myPWS.Outputs.Item(myPWS.Outputs.Name(1))
            myPWS.System.ControlMode = TekPWS4000ControlModeEnum.TekPWS4000ControlModeRemote
            myPWSOutPut.Enabled = False
            myPWSOutPut.VoltageLevel = OperationV
            myPWSOutPut.CurrentLimit = 5.0
            myPWSOutPut.Enabled = True
        Else
            myPWSOutPut = myPWS.Outputs.Item(myPWS.Outputs.Name(1))
            myPWS.System.ControlMode = TekPWS4000ControlModeEnum.TekPWS4000ControlModeRemote
            myPWSOutPut.Enabled = False
            myPWSOutPut.VoltageLevel = OperationV
            myPWSOutPut.CurrentLimit = 5.0
            myPWSOutPut.Enabled = True
        End If
    End Sub

    Public Sub PSU15V()
        If myPWS.Initialized = False Then
            myPWS.Initialize("USB0::0x0699::0x0390::C010255::0::INSTR", True, False)
            myPWSOutPut = myPWS.Outputs.Item(myPWS.Outputs.Name(1))
            myPWS.System.ControlMode = TekPWS4000ControlModeEnum.TekPWS4000ControlModeRemote
            myPWSOutPut.Enabled = False
            myPWSOutPut.VoltageLevel = BootmodeV
            myPWSOutPut.CurrentLimit = 5.0
            myPWSOutPut.Enabled = True
        Else
            myPWSOutPut = myPWS.Outputs.Item(myPWS.Outputs.Name(1))
            myPWS.System.ControlMode = TekPWS4000ControlModeEnum.TekPWS4000ControlModeRemote
            myPWSOutPut.Enabled = False
            myPWSOutPut.VoltageLevel = BootmodeV
            myPWSOutPut.CurrentLimit = 5.0
            myPWSOutPut.Enabled = True
        End If
    End Sub

    Public Sub PSU15AV()
        If myPWS.Initialized = False Then
            myPWS.Initialize("USB0::0x0699::0x0390::C010255::0::INSTR", True, False)
            myPWSOutPut = myPWS.Outputs.Item(myPWS.Outputs.Name(1))
            myPWS.System.ControlMode = TekPWS4000ControlModeEnum.TekPWS4000ControlModeRemote
            myPWSOutPut.Enabled = False
            myPWSOutPut.VoltageLevel = 15.95
            myPWSOutPut.CurrentLimit = 5.0
            myPWSOutPut.Enabled = True
        Else
            myPWSOutPut = myPWS.Outputs.Item(myPWS.Outputs.Name(1))
            myPWS.System.ControlMode = TekPWS4000ControlModeEnum.TekPWS4000ControlModeRemote
            myPWSOutPut.Enabled = False
            myPWSOutPut.VoltageLevel = 15.95
            myPWSOutPut.CurrentLimit = 5.0
            myPWSOutPut.Enabled = True
        End If
    End Sub

    Public Sub PSU15BV()
        If myPWS.Initialized = False Then
            myPWS.Initialize("USB0::0x0699::0x0390::C010255::0::INSTR", True, False)
            myPWSOutPut = myPWS.Outputs.Item(myPWS.Outputs.Name(1))
            myPWS.System.ControlMode = TekPWS4000ControlModeEnum.TekPWS4000ControlModeRemote
            myPWSOutPut.Enabled = False
            myPWSOutPut.VoltageLevel = 15.75
            myPWSOutPut.CurrentLimit = 5.0
            myPWSOutPut.Enabled = True
        Else
            myPWSOutPut = myPWS.Outputs.Item(myPWS.Outputs.Name(1))
            myPWS.System.ControlMode = TekPWS4000ControlModeEnum.TekPWS4000ControlModeRemote
            myPWSOutPut.Enabled = False
            myPWSOutPut.VoltageLevel = 15.75
            myPWSOutPut.CurrentLimit = 5.0
            myPWSOutPut.Enabled = True
        End If
    End Sub

    Public Sub PSU18V()
        If myPWS.Initialized = False Then
            myPWS.Initialize("USB0::0x0699::0x0390::C010255::0::INSTR", True, False)
            myPWSOutPut = myPWS.Outputs.Item(myPWS.Outputs.Name(1))
            myPWS.System.ControlMode = TekPWS4000ControlModeEnum.TekPWS4000ControlModeRemote
            myPWSOutPut.Enabled = False
            myPWSOutPut.VoltageLevel = 18
            myPWSOutPut.CurrentLimit = 5.0
            myPWSOutPut.Enabled = True
        Else
            myPWSOutPut = myPWS.Outputs.Item(myPWS.Outputs.Name(1))
            myPWS.System.ControlMode = TekPWS4000ControlModeEnum.TekPWS4000ControlModeRemote
            myPWSOutPut.Enabled = False
            myPWSOutPut.VoltageLevel = 18
            myPWSOutPut.CurrentLimit = 5.0
            myPWSOutPut.Enabled = True
        End If
    End Sub

    Private Sub ButtonSaveV_Click(sender As Object, e As EventArgs) Handles ButtonSaveV.Click
        BootmodeV = Convert.ToDecimal(TextBoxBootmodeV.Text)
        OperationV = Convert.ToDecimal(TextBoxOperationV.Text)

        My.Settings.BootmodeV = BootmodeV
        My.Settings.OperationV = OperationV
        My.Settings.Save()
    End Sub

    '===== CIM =====

    Private Sub CheckBoxSendCIM_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxSendCIM.CheckedChanged
        If CheckBoxSendCIM.Checked = True Then
            LabelInfoCIM.Text = "SEND CIM DATA"
            LabelInfoCIM.BackColor = Color.Brown
        Else
            LabelInfoCIM.Text = "DON'T SEND CIM DATA"
            LabelInfoCIM.BackColor = Color.Red
        End If
    End Sub

    Public Sub Write101N1()
        Delay_Ms(100)
        FileOpen(File, "C:\cimlibrary\FUN-101\to_cim\101.txt", OpenMode.Append)
        PrintLine(File, "|" & BarcodeN1 & "|*|")
        FileClose(File)
    End Sub

    Public Sub Write101N2()
        Delay_Ms(100)
        FileOpen(File, "C:\cimlibrary\FUN-101\to_cim\101.txt", OpenMode.Append)
        PrintLine(File, "|" & BarcodeN2 & "|*|")
        FileClose(File)
    End Sub

    Public Sub Write101N3()
        Delay_Ms(100)
        FileOpen(File, "C:\cimlibrary\FUN-101\to_cim\101.txt", OpenMode.Append)
        PrintLine(File, "|" & BarcodeN3 & "|*|")
        FileClose(File)
    End Sub

    Public Sub Write101N4()
        Delay_Ms(100)
        FileOpen(File, "C:\cimlibrary\FUN-101\to_cim\101.txt", OpenMode.Append)
        PrintLine(File, "|" & BarcodeN4 & "|*|")
        FileClose(File)
    End Sub

    Public Sub Read102()
        Delay_Ms(300)
        If Dir("C:\cimlibrary\FUN-101\to_dev\102.txt") = "" Then
            Do Until Dir("C:\cimlibrary\FUN-101\to_dev\102.txt") <> ""
                Delay_Ms(1000)
                LoopDir = LoopDir + 1
                If LoopDir > 3 Then
                    MsgBox("CIM File Not Found!")
                    TextBoxBarcode.Clear()
                    TextBoxBarcode.Focus()
                    TextBoxBarcode.ReadOnly = False
                    TextBoxBarcode.BackColor = Color.White
                    Exit Do
                End If
            Loop
        End If

        If Dir("C:\cimlibrary\FUN-101\to_dev\102.txt") <> "" Then
            Using SR As New StreamReader("C:\cimlibrary\FUN-101\to_dev\102.txt")
                Content102 = SR.ReadToEnd()
                Condition = Mid(Content102, 31, 1)
            End Using

            If Condition = "P" Then
                CIMRun = True
            ElseIf Condition = "F" Then
                CIMRun = False
            End If
        End If
    End Sub

    Public Sub Nest1NG()
        If ProcessN1 = False Then
            LabelJudgementN1.BackColor = Color.Red
            LabelJudgementN1.Text = "NG!"
            RecheckStepN1 = False
            BootmodeN1Off()
            Delay_Ms(100)
            AllLEDN1Off()
            Delay_Ms(100)
            If CheckBoxSendCIM.Checked = True Then
                Write200N1()
            End If
        End If
    End Sub

    Public Sub Write100N1()
        Delay_Ms(250)
        FileOpen(File, "C:\cimlibrary\FUN-101\to_cim\100.txt", OpenMode.Append)
        PrintLine(File, "|" & BarcodeN1 & "|*|P|")
        FileClose(File)
    End Sub

    'Public Sub NoCimOkN1()
    '    Dim cmd As MySqlCommand
    '    Dim timeStamp As DateTime = DateTime.Now
    '    con.Open()
    '    cmd = con.CreateCommand
    '    cmd.CommandText = "select serial_number from fct where serial_number = '" + BarcodeN1 + "'"
    '    Dim lrd As MySqlDataReader = cmd.ExecuteReader()
    '    If lrd.HasRows Then
    '        con.Close()
    '        Delay_Ms(100)
    '        con.Open()
    '        cmd = con.CreateCommand
    '        cmd.CommandText = "update fct set source = 'FUN-101 N1', created_at = @created where serial_number = '" + BarcodeN1 + "'"
    '        cmd.Parameters.AddWithValue("@created", DateTime.Now)
    '        cmd.ExecuteNonQuery()
    '        con.Close()
    '        Delay_Ms(100)
    '    Else
    '        con.Close()
    '        Delay_Ms(100)
    '        con.Open()
    '        cmd = con.CreateCommand
    '        cmd.CommandText = "insert into fct(paco_id ,serial_number, source, created_at)values(@paco_id,@sernum,@source,@created);"
    '        cmd.Parameters.AddWithValue("@paco_id", Nothing)
    '        cmd.Parameters.AddWithValue("@source", "FUN-101 N1")
    '        cmd.Parameters.AddWithValue("@sernum", BarcodeN1)
    '        cmd.Parameters.AddWithValue("@created", DateTime.Now)
    '        cmd.ExecuteNonQuery()
    '        con.Close()
    '        Delay_Ms(100)
    '    End If
    'End Sub

    Public Sub Nest2NG()
        If ProcessN2 = False Then
            LabelJudgementN2.BackColor = Color.Red
            LabelJudgementN2.Text = "NG!"
            RecheckStepN2 = False
            BootmodeN2Off()
            Delay_Ms(100)
            AllLEDN2Off()
            Delay_Ms(100)
            If CheckBoxSendCIM.Checked = True Then
                Write200N2()
            End If
        End If
    End Sub

    Public Sub Write100N2()
        Delay_Ms(250)
        FileOpen(File, "C:\cimlibrary\FUN-101\to_cim\100.txt", OpenMode.Append)
        PrintLine(File, "|" & BarcodeN2 & "|*|P|")
        FileClose(File)
    End Sub

    'Public Sub NoCimOkN2()
    '    Dim cmd As MySqlCommand
    '    Dim timeStamp As DateTime = DateTime.Now
    '    con.Open()
    '    cmd = con.CreateCommand
    '    cmd.CommandText = "select serial_number from fct where serial_number = '" + BarcodeN2 + "'"
    '    Dim lrd As MySqlDataReader = cmd.ExecuteReader()
    '    If lrd.HasRows Then
    '        con.Close()
    '        Delay_Ms(100)
    '        con.Open()
    '        cmd = con.CreateCommand
    '        cmd.CommandText = "update fct set source = 'FUN-101 N2', created_at = @created where serial_number = '" + BarcodeN2 + "'"
    '        cmd.Parameters.AddWithValue("@created", DateTime.Now)
    '        cmd.ExecuteNonQuery()
    '        con.Close()
    '        Delay_Ms(100)
    '    Else
    '        con.Close()
    '        Delay_Ms(100)
    '        con.Open()
    '        cmd = con.CreateCommand
    '        cmd.CommandText = "insert into fct(paco_id ,serial_number, source, created_at)values(@paco_id,@sernum,@source,@created);"
    '        cmd.Parameters.AddWithValue("@paco_id", Nothing)
    '        cmd.Parameters.AddWithValue("@source", "FUN-101 N2")
    '        cmd.Parameters.AddWithValue("@sernum", BarcodeN2)
    '        cmd.Parameters.AddWithValue("@created", DateTime.Now)
    '        cmd.ExecuteNonQuery()
    '        con.Close()
    '        Delay_Ms(100)
    '    End If
    'End Sub

    Public Sub Nest3NG()
        If ProcessN3 = False Then
            LabelJudgementN3.BackColor = Color.Red
            LabelJudgementN3.Text = "NG!"
            RecheckStepN3 = False
            BootmodeN3Off()
            Delay_Ms(100)
            AllLEDN3Off()
            Delay_Ms(100)
            If CheckBoxSendCIM.Checked = True Then
                Write200N3()
            End If
        End If
    End Sub

    Public Sub Write100N3()
        Delay_Ms(250)
        FileOpen(File, "C:\cimlibrary\FUN-101\to_cim\100.txt", OpenMode.Append)
        PrintLine(File, "|" & BarcodeN3 & "|*|P|")
        FileClose(File)
    End Sub

    'Public Sub NoCimOkN3()
    '    Dim cmd As MySqlCommand
    '    Dim timeStamp As DateTime = DateTime.Now
    '    con.Open()
    '    cmd = con.CreateCommand
    '    cmd.CommandText = "select serial_number from fct where serial_number = '" + BarcodeN3 + "'"
    '    Dim lrd As MySqlDataReader = cmd.ExecuteReader()
    '    If lrd.HasRows Then
    '        con.Close()
    '        Delay_Ms(100)
    '        con.Open()
    '        cmd = con.CreateCommand
    '        cmd.CommandText = "update fct set source = 'FUN-101 N3', created_at = @created where serial_number = '" + BarcodeN3 + "'"
    '        cmd.Parameters.AddWithValue("@created", DateTime.Now)
    '        cmd.ExecuteNonQuery()
    '        con.Close()
    '        Delay_Ms(100)
    '    Else
    '        con.Close()
    '        con.Open()
    '        cmd = con.CreateCommand
    '        cmd.CommandText = "insert into fct(paco_id ,serial_number, source, created_at)values(@paco_id,@sernum,@source,@created);"
    '        cmd.Parameters.AddWithValue("@paco_id", Nothing)
    '        cmd.Parameters.AddWithValue("@source", "FUN-101 N3")
    '        cmd.Parameters.AddWithValue("@sernum", BarcodeN3)
    '        cmd.Parameters.AddWithValue("@created", DateTime.Now)
    '        cmd.ExecuteNonQuery()
    '        con.Close()
    '        Delay_Ms(100)
    '    End If
    'End Sub

    Public Sub Nest4NG()
        If ProcessN4 = False Then
            LabelJudgementN4.BackColor = Color.Red
            LabelJudgementN4.Text = "NG!"
            RecheckStepN4 = False
            BootmodeN4Off()
            Delay_Ms(100)
            AllLEDN4Off()
            Delay_Ms(100)
            If CheckBoxSendCIM.Checked = True Then
                Write200N4()
            End If
        End If
    End Sub

    Public Sub Write100N4()
        Delay_Ms(250)
        FileOpen(File, "C:\cimlibrary\FUN-101\to_cim\100.txt", OpenMode.Append)
        PrintLine(File, "|" & BarcodeN4 & "|*|P|")
        FileClose(File)
    End Sub

    'Public Sub NoCimOkN4()
    '    Dim cmd As MySqlCommand
    '    Dim timeStamp As DateTime = DateTime.Now
    '    con.Open()
    '    cmd = con.CreateCommand
    '    cmd.CommandText = "select serial_number from fct where serial_number = '" + BarcodeN4 + "'"
    '    Dim lrd As MySqlDataReader = cmd.ExecuteReader()
    '    If lrd.HasRows Then
    '        con.Close()
    '        Delay_Ms(100)
    '        con.Open()
    '        cmd = con.CreateCommand
    '        cmd.CommandText = "update fct set source = 'FUN-101 N4', created_at = @created where serial_number = '" + BarcodeN4 + "'"
    '        cmd.Parameters.AddWithValue("@created", DateTime.Now)
    '        cmd.ExecuteNonQuery()
    '        con.Close()
    '        Delay_Ms(100)
    '    Else
    '        con.Close()
    '        Delay_Ms(100)
    '        con.Open()
    '        cmd = con.CreateCommand
    '        cmd.CommandText = "insert into fct(paco_id ,serial_number, source, created_at)values(@paco_id,@sernum,@source,@created);"
    '        cmd.Parameters.AddWithValue("@paco_id", Nothing)
    '        cmd.Parameters.AddWithValue("@source", "FUN-101 N4")
    '        cmd.Parameters.AddWithValue("@sernum", BarcodeN4)
    '        cmd.Parameters.AddWithValue("@created", DateTime.Now)
    '        cmd.ExecuteNonQuery()
    '        con.Close()
    '        Delay_Ms(100)
    '    End If
    'End Sub

    Public Sub Write200N1()
        Delay_Ms(25)
        FileOpen(File, "C:\cimlibrary\FUN-101\to_cim\200.txt", OpenMode.Append)
        PrintLine(File, "|" & BarcodeN1 & NGType)
        FileClose(File)
    End Sub

    Public Sub Write200N2()
        Delay_Ms(25)
        FileOpen(File, "C:\cimlibrary\FUN-101\to_cim\200.txt", OpenMode.Append)
        PrintLine(File, "|" & BarcodeN2 & NGType)
        FileClose(File)
    End Sub

    Public Sub Write200N3()
        Delay_Ms(25)
        FileOpen(File, "C:\cimlibrary\FUN-101\to_cim\200.txt", OpenMode.Append)
        PrintLine(File, "|" & BarcodeN3 & NGType)
        FileClose(File)
    End Sub

    Public Sub Write200N4()
        Delay_Ms(25)
        FileOpen(File, "C:\cimlibrary\FUN-101\to_cim\200.txt", OpenMode.Append)
        PrintLine(File, "|" & BarcodeN4 & NGType)
        FileClose(File)
    End Sub

    '===== SERIAL COMMAND =====

    Public Sub OnComPWBAN1()
        If SerialPWBANest1.PortOpen = False Then
            SerialPWBANest1.PortOpen = True
        End If
    End Sub

    Public Sub OffComPWBAN1()
        If SerialPWBANest1.PortOpen = True Then
            SerialPWBANest1.PortOpen = False
        End If
    End Sub

    Public Sub OnComPWBAN2()
        If SerialPWBANest2.PortOpen = False Then
            SerialPWBANest2.PortOpen = True
        End If
    End Sub

    Public Sub OffComPWBAN2()
        If SerialPWBANest2.PortOpen = True Then
            SerialPWBANest2.PortOpen = False
        End If
    End Sub

    Public Sub OnComPWBAN3()
        If SerialPWBANest3.PortOpen = False Then
            SerialPWBANest3.PortOpen = True
        End If
    End Sub

    Public Sub OffComPWBAN3()
        If SerialPWBANest3.PortOpen = True Then
            SerialPWBANest3.PortOpen = False
        End If
    End Sub

    Public Sub OnComPWBAN4()
        If SerialPWBANest4.PortOpen = False Then
            SerialPWBANest4.PortOpen = True
        End If
    End Sub

    Public Sub OffComPWBAN4()
        If SerialPWBANest4.PortOpen = True Then
            SerialPWBANest4.PortOpen = False
        End If
    End Sub

    Public Sub CheckROMN1()
        OnComPWBAN1()
        SerialPWBANest1.InBufferCount = 0
        SerialPWBANest1.Output = "V"
        'Delay_Ms(200)
        Thread.Sleep(200)
        tempbuffN1 = ""
        tempstrN1 = ""
        strbuffN1 = ""
        inbuffcntN1 = 0
        inbufflenN1 = 0
        strhexbuffN1(1000) = ""
        strhexbuffN1(4) = 0
        strhexbuffN1(5) = 0
        If SerialPWBANest1.InBufferCount <> 0 Then
            Do While SerialPWBANest1.InBufferCount
                inbuffcntN1 = inbuffcntN1 + SerialPWBANest1.InBufferCount
                tempbuffN1 = tempbuffN1 + SerialPWBANest1.Input
            Loop
        End If
        If inbuffcntN1 > 0 Then
            inbufflenN1 = Len(tempbuffN1)
            For i = 0 To (inbufflenN1 - 1)
                tempstrN1 = Mid(tempbuffN1, i + 1, 2)
                If Asc(tempstrN1) < 16 Then
                    strhexbuffN1(i) = "0" + Hex(Asc(tempstrN1))
                Else
                    strhexbuffN1(i) = Hex(Asc(tempstrN1))
                End If
            Next i
        End If
        OffComPWBAN1()
    End Sub

    Public Sub CheckROMN2()
        OnComPWBAN2()
        SerialPWBANest2.InBufferCount = 0
        SerialPWBANest2.Output = "V"
        'Delay_Ms(200)
        Thread.Sleep(200)
        tempbuffN2 = ""
        tempstrN2 = ""
        strbuffN2 = ""
        inbuffcntN2 = 0
        inbufflenN2 = 0
        strhexbuffN2(1000) = ""
        strhexbuffN2(4) = 0
        strhexbuffN2(5) = 0
        If SerialPWBANest2.InBufferCount <> 0 Then
            Do While SerialPWBANest2.InBufferCount
                inbuffcntN2 = inbuffcntN2 + SerialPWBANest2.InBufferCount
                tempbuffN2 = tempbuffN2 + SerialPWBANest2.Input
            Loop
        End If
        If inbuffcntN2 > 0 Then
            inbufflenN2 = Len(tempbuffN2)
            For i = 0 To (inbufflenN2 - 1)
                tempstrN2 = Mid(tempbuffN2, i + 1, 2)
                If Asc(tempstrN2) < 16 Then
                    strhexbuffN2(i) = "0" + Hex(Asc(tempstrN2))
                Else
                    strhexbuffN2(i) = Hex(Asc(tempstrN2))
                End If
            Next i
        End If
        OffComPWBAN2()
    End Sub

    Public Sub CheckROMN3()
        OnComPWBAN3()
        SerialPWBANest3.InBufferCount = 0
        SerialPWBANest3.Output = "V"
        'Delay_Ms(200)
        Thread.Sleep(200)
        tempbuffN3 = ""
        tempstrN3 = ""
        strbuffN3 = ""
        inbuffcntN3 = 0
        inbufflenN3 = 0
        strhexbuffN3(1000) = ""
        strhexbuffN3(4) = 0
        strhexbuffN3(5) = 0
        If SerialPWBANest3.InBufferCount <> 0 Then
            Do While SerialPWBANest3.InBufferCount
                inbuffcntN3 = inbuffcntN3 + SerialPWBANest3.InBufferCount
                tempbuffN3 = tempbuffN3 + SerialPWBANest3.Input
            Loop
        End If
        If inbuffcntN3 > 0 Then
            inbufflenN3 = Len(tempbuffN3)
            For i = 0 To (inbufflenN3 - 1)
                tempstrN3 = Mid(tempbuffN3, i + 1, 2)
                If Asc(tempstrN3) < 16 Then
                    strhexbuffN3(i) = "0" + Hex(Asc(tempstrN3))
                Else
                    strhexbuffN3(i) = Hex(Asc(tempstrN3))
                End If
            Next i
        End If
        OffComPWBAN3()
    End Sub

    Public Sub CheckROMN4()
        OnComPWBAN4()
        SerialPWBANest4.InBufferCount = 0
        SerialPWBANest4.Output = "V"
        'Delay_Ms(200)
        Thread.Sleep(200)
        tempbuffN4 = ""
        tempstrN4 = ""
        strbuffN4 = ""
        inbuffcntN4 = 0
        inbufflenN4 = 0
        strhexbuffN4(1000) = ""
        strhexbuffN4(4) = 0
        strhexbuffN4(5) = 0
        If SerialPWBANest4.InBufferCount <> 0 Then
            Do While SerialPWBANest4.InBufferCount
                inbuffcntN4 = inbuffcntN4 + SerialPWBANest4.InBufferCount
                tempbuffN4 = tempbuffN4 + SerialPWBANest4.Input
            Loop
        End If
        If inbuffcntN4 > 0 Then
            inbufflenN4 = Len(tempbuffN4)
            For i = 0 To (inbufflenN4 - 1)
                tempstrN4 = Mid(tempbuffN4, i + 1, 2)
                If Asc(tempstrN4) < 16 Then
                    strhexbuffN4(i) = "0" + Hex(Asc(tempstrN4))
                Else
                    strhexbuffN4(i) = Hex(Asc(tempstrN4))
                End If
            Next i
        End If
        OffComPWBAN4()
    End Sub

    Public Sub CheckNVMN1()
        OnComPWBAN1()
        SerialPWBANest1.InBufferCount = 0
        SerialPWBANest1.Output = "N"
        Delay_Ms(1000)
        'Thread.Sleep(1200)
        tempbuffN1 = ""
        tempstrN1 = ""
        strbuffN1 = ""
        inbuffcntN1 = 0
        inbufflenN1 = 0
        strhexbuffN1(520) = ""
        strhexbuffN1(4) = 0
        strhexbuffN1(5) = 0
        If SerialPWBANest1.InBufferCount <> 0 Then
            Do While SerialPWBANest1.InBufferCount
                inbuffcntN1 = inbuffcntN1 + SerialPWBANest1.InBufferCount
                tempbuffN1 = tempbuffN1 + SerialPWBANest1.Input
            Loop
        End If
        If inbuffcntN1 > 0 Then
            inbufflenN1 = Len(tempbuffN1)
            For i = 0 To (inbufflenN1 - 1)
                tempstrN1 = Mid(tempbuffN1, i + 1, 2)
                If Asc(tempstrN1) < 16 Then
                    strhexbuffN1(i) = "0" + Hex(Asc(tempstrN1))
                Else
                    strhexbuffN1(i) = Hex(Asc(tempstrN1))
                End If
            Next i
        End If
        OffComPWBAN1()
    End Sub

    Public Sub CheckNVMN2()
        OnComPWBAN2()
        SerialPWBANest2.InBufferCount = 0
        SerialPWBANest2.Output = "N"
        Delay_Ms(1000)
        'Thread.Sleep(1200)
        tempbuffN2 = ""
        tempstrN2 = ""
        strbuffN2 = ""
        inbuffcntN2 = 0
        inbufflenN2 = 0
        strhexbuffN2(520) = ""
        strhexbuffN2(4) = 0
        strhexbuffN2(5) = 0
        If SerialPWBANest2.InBufferCount <> 0 Then
            Do While SerialPWBANest2.InBufferCount
                inbuffcntN2 = inbuffcntN2 + SerialPWBANest2.InBufferCount
                tempbuffN2 = tempbuffN2 + SerialPWBANest2.Input
            Loop
        End If
        If inbuffcntN2 > 0 Then
            inbufflenN2 = Len(tempbuffN2)
            For i = 0 To (inbufflenN2 - 1)
                tempstrN2 = Mid(tempbuffN2, i + 1, 2)
                If Asc(tempstrN2) < 16 Then
                    strhexbuffN2(i) = "0" + Hex(Asc(tempstrN2))
                Else
                    strhexbuffN2(i) = Hex(Asc(tempstrN2))
                End If
            Next i
        End If
        OffComPWBAN2()
    End Sub

    Public Sub CheckNVMN3()
        OnComPWBAN3()
        SerialPWBANest3.InBufferCount = 0
        SerialPWBANest3.Output = "N"
        Delay_Ms(1000)
        'Thread.Sleep(1200)
        tempbuffN3 = ""
        tempstrN3 = ""
        strbuffN3 = ""
        inbuffcntN3 = 0
        inbufflenN3 = 0
        strhexbuffN3(520) = ""
        strhexbuffN3(4) = 0
        strhexbuffN3(5) = 0
        If SerialPWBANest3.InBufferCount <> 0 Then
            Do While SerialPWBANest3.InBufferCount
                inbuffcntN3 = inbuffcntN3 + SerialPWBANest3.InBufferCount
                tempbuffN3 = tempbuffN3 + SerialPWBANest3.Input
            Loop
        End If
        If inbuffcntN3 > 0 Then
            inbufflenN3 = Len(tempbuffN3)
            For i = 0 To (inbufflenN3 - 1)
                tempstrN3 = Mid(tempbuffN3, i + 1, 2)
                If Asc(tempstrN3) < 16 Then
                    strhexbuffN3(i) = "0" + Hex(Asc(tempstrN3))
                Else
                    strhexbuffN3(i) = Hex(Asc(tempstrN3))
                End If
            Next i
        End If
        OffComPWBAN3()
    End Sub

    Public Sub CheckNVMN4()
        OnComPWBAN4()
        SerialPWBANest4.InBufferCount = 0
        SerialPWBANest4.Output = "N"
        Delay_Ms(1000)
        'Thread.Sleep(1200)
        tempbuffN4 = ""
        tempstrN4 = ""
        strbuffN4 = ""
        inbuffcntN4 = 0
        inbufflenN4 = 0
        strhexbuffN4(520) = ""
        strhexbuffN4(4) = 0
        strhexbuffN4(5) = 0
        If SerialPWBANest4.InBufferCount <> 0 Then
            Do While SerialPWBANest4.InBufferCount
                inbuffcntN4 = inbuffcntN4 + SerialPWBANest4.InBufferCount
                tempbuffN4 = tempbuffN4 + SerialPWBANest4.Input
            Loop
        End If
        If inbuffcntN4 > 0 Then
            inbufflenN4 = Len(tempbuffN4)
            For i = 0 To (inbufflenN4 - 1)
                tempstrN4 = Mid(tempbuffN4, i + 1, 2)
                If Asc(tempstrN4) < 16 Then
                    strhexbuffN4(i) = "0" + Hex(Asc(tempstrN4))
                Else
                    strhexbuffN4(i) = Hex(Asc(tempstrN4))
                End If
            Next i
        End If
        OffComPWBAN4()
    End Sub

    Public Sub UnlockN1()
        OnComPWBAN1()
        SerialPWBANest1.Output = "u"
        Delay_Ms(1)
        SerialPWBANest1.Output = "n"
        Delay_Ms(1)
        SerialPWBANest1.Output = "l"
        Delay_Ms(1)
        SerialPWBANest1.Output = "o"
        Delay_Ms(1)
        SerialPWBANest1.Output = "c"
        Delay_Ms(1)
        SerialPWBANest1.Output = "k"
        Delay_Ms(1)
    End Sub

    Public Sub UnlockN2()
        OnComPWBAN2()
        SerialPWBANest2.Output = "u"
        Delay_Ms(1)
        SerialPWBANest2.Output = "n"
        Delay_Ms(1)
        SerialPWBANest2.Output = "l"
        Delay_Ms(1)
        SerialPWBANest2.Output = "o"
        Delay_Ms(1)
        SerialPWBANest2.Output = "c"
        Delay_Ms(1)
        SerialPWBANest2.Output = "k"
        Delay_Ms(1)
    End Sub

    Public Sub UnlockN3()
        OnComPWBAN3()
        SerialPWBANest3.Output = "u"
        Delay_Ms(1)
        SerialPWBANest3.Output = "n"
        Delay_Ms(1)
        SerialPWBANest3.Output = "l"
        Delay_Ms(1)
        SerialPWBANest3.Output = "o"
        Delay_Ms(1)
        SerialPWBANest3.Output = "c"
        Delay_Ms(1)
        SerialPWBANest3.Output = "k"
        Delay_Ms(1)
    End Sub

    Public Sub UnlockN4()
        OnComPWBAN4()
        SerialPWBANest4.Output = "u"
        Delay_Ms(1)
        SerialPWBANest4.Output = "n"
        Delay_Ms(1)
        SerialPWBANest4.Output = "l"
        Delay_Ms(1)
        SerialPWBANest4.Output = "o"
        Delay_Ms(1)
        SerialPWBANest4.Output = "c"
        Delay_Ms(1)
        SerialPWBANest4.Output = "k"
        Delay_Ms(1)
    End Sub

    Public Sub SendPALLN1()
        OnComPWBAN1()
        Dim PALL(3) As Byte
        PALL(0) = &H50     'P
        PALL(1) = &H41     'A
        PALL(2) = &H4C     'L
        PALL(3) = &H4C     'L
        SerialPWBANest1.Output = PALL
        Delay_Ms(50)
    End Sub

    Public Sub SendPALLN2()
        OnComPWBAN2()
        Dim PALL(3) As Byte
        PALL(0) = &H50     'P
        PALL(1) = &H41     'A
        PALL(2) = &H4C     'L
        PALL(3) = &H4C     'L
        SerialPWBANest2.Output = PALL
        Delay_Ms(50)
    End Sub

    Public Sub SendPALLN3()
        OnComPWBAN3()
        Dim PALL(3) As Byte
        PALL(0) = &H50     'P
        PALL(1) = &H41     'A
        PALL(2) = &H4C     'L
        PALL(3) = &H4C     'L
        SerialPWBANest3.Output = PALL
        Delay_Ms(50)
    End Sub

    Public Sub SendPALLN4()
        OnComPWBAN4()
        Dim PALL(3) As Byte
        PALL(0) = &H50     'P
        PALL(1) = &H41     'A
        PALL(2) = &H4C     'L
        PALL(3) = &H4C     'L
        SerialPWBANest4.Output = PALL
        Delay_Ms(50)
    End Sub

    Public Sub WriteNVMN1()
        OnComPWBAN1()

        Dim Send(15) As Byte
        Send(0) = &HFF
        Send(1) = &H0
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H1
        Send(5) = &H6
        Send(6) = &H1
        Send(7) = &H0
        Send(8) = &HF6
        Send(9) = &H0
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H27
        Send(13) = &H8D
        Send(14) = &HA
        Send(15) = &H9
        SerialPWBANest1.Output = Send '1	   
        Delay_Ms(50)

        Send(0) = &H12
        Send(1) = &HFF
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H80
        Send(5) = &H3E
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HD0
        Send(11) = &H7
        Send(12) = &HFC
        Send(13) = &H8
        Send(14) = &HA0
        Send(15) = &HF
        SerialPWBANest1.Output = Send '2	   
        Delay_Ms(50)

        Send(0) = &H94
        Send(1) = &H11
        Send(2) = &H70
        Send(3) = &H17
        Send(4) = &H2C
        Send(5) = &H1A
        Send(6) = &H40
        Send(7) = &H1F
        Send(8) = &HC4
        Send(9) = &H22
        Send(10) = &H10
        Send(11) = &H27
        Send(12) = &HC0
        Send(13) = &H2B
        Send(14) = &HE0
        Send(15) = &H2E
        SerialPWBANest1.Output = Send '3	   
        Delay_Ms(50)

        Send(0) = &H58
        Send(1) = &H34
        Send(2) = &HB0
        Send(3) = &H36
        Send(4) = &HBE
        Send(5) = &H3C
        Send(6) = &H80
        Send(7) = &H3E
        Send(8) = &H8E
        Send(9) = &H44
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest1.Output = Send '4	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H80
        Send(5) = &H3E
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HD0
        Send(11) = &H7
        Send(12) = &HFC
        Send(13) = &H8
        Send(14) = &HA0
        Send(15) = &HF
        SerialPWBANest1.Output = Send '5	   
        Delay_Ms(50)

        Send(0) = &H94
        Send(1) = &H11
        Send(2) = &H70
        Send(3) = &H17
        Send(4) = &H2C
        Send(5) = &H1A
        Send(6) = &H40
        Send(7) = &H1F
        Send(8) = &HC4
        Send(9) = &H22
        Send(10) = &H10
        Send(11) = &H27
        Send(12) = &HC0
        Send(13) = &H2B
        Send(14) = &HE0
        Send(15) = &H2E
        SerialPWBANest1.Output = Send '6	   
        Delay_Ms(50)

        Send(0) = &H58
        Send(1) = &H34
        Send(2) = &HB0
        Send(3) = &H36
        Send(4) = &HBE
        Send(5) = &H3C
        Send(6) = &H80
        Send(7) = &H3E
        Send(8) = &H8E
        Send(9) = &H44
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest1.Output = Send '7	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H80
        Send(5) = &H3E
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HD0
        Send(11) = &H7
        Send(12) = &H66
        Send(13) = &H8
        Send(14) = &HA0
        Send(15) = &HF
        SerialPWBANest1.Output = Send '8	   
        Delay_Ms(50)

        Send(0) = &HCC
        Send(1) = &H10
        Send(2) = &H70
        Send(3) = &H17
        Send(4) = &H64
        Send(5) = &H19
        Send(6) = &H40
        Send(7) = &H1F
        Send(8) = &HFC
        Send(9) = &H21
        Send(10) = &H10
        Send(11) = &H27
        Send(12) = &HF8
        Send(13) = &H2A
        Send(14) = &HE0
        Send(15) = &H2E
        SerialPWBANest1.Output = Send '9	   
        Delay_Ms(50)

        Send(0) = &H90
        Send(1) = &H33
        Send(2) = &HB0
        Send(3) = &H36
        Send(4) = &H8C
        Send(5) = &H3C
        Send(6) = &H80
        Send(7) = &H3E
        Send(8) = &H5C
        Send(9) = &H44
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest1.Output = Send '10	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &H4A
        Send(3) = &H6D
        Send(4) = &H4A
        Send(5) = &H6D
        Send(6) = &H2
        Send(7) = &H5
        Send(8) = &H32
        Send(9) = &H0
        Send(10) = &H32
        Send(11) = &H0
        Send(12) = &H4
        Send(13) = &H0
        Send(14) = &HB7
        Send(15) = &H3F
        SerialPWBANest1.Output = Send '11	   
        Delay_Ms(50)

        Send(0) = &H48
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &HFF
        Send(5) = &HFF
        Send(6) = &HFF
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HA4
        Send(11) = &H70
        Send(12) = &H3D
        Send(13) = &H0
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest1.Output = Send '12	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H19
        Send(4) = &HFF
        Send(5) = &HFF
        Send(6) = &HFF
        Send(7) = &HFF
        Send(8) = &H1
        Send(9) = &H0
        Send(10) = &H1
        Send(11) = &H0
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &H18
        Send(15) = &H3
        SerialPWBANest1.Output = Send '13	   
        Delay_Ms(50)

        Send(0) = &HE3
        Send(1) = &H0
        Send(2) = &HA
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &HFF
        Send(6) = &HE3
        Send(7) = &H0
        Send(8) = &HBE
        Send(9) = &H2
        Send(10) = &HE3
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H4C
        Send(15) = &H1
        SerialPWBANest1.Output = Send '14	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H2A
        Send(2) = &H8D
        Send(3) = &H1
        Send(4) = &H0
        Send(5) = &H55
        Send(6) = &HD2
        Send(7) = &H1
        Send(8) = &H0
        Send(9) = &H7F
        Send(10) = &H19
        Send(11) = &H2
        Send(12) = &H0
        Send(13) = &HA9
        Send(14) = &H66
        Send(15) = &H2
        SerialPWBANest1.Output = Send '15	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &HD3
        Send(2) = &HBE
        Send(3) = &H2
        Send(4) = &H0
        Send(5) = &HFE
        Send(6) = &H3F
        Send(7) = &HFF
        Send(8) = &HF2
        Send(9) = &HF2
        Send(10) = &HF2
        Send(11) = &HF2
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest1.Output = Send '16	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &H3F
        Send(7) = &H3F
        Send(8) = &H3F
        Send(9) = &H3F
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest1.Output = Send '17	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &H3F
        Send(7) = &H3F
        Send(8) = &H3F
        Send(9) = &H3F
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest1.Output = Send '18	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &H3F
        Send(7) = &H3F
        Send(8) = &H3F
        Send(9) = &H3F
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest1.Output = Send '19	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &HE8
        Send(7) = &H3
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest1.Output = Send '20	   
        Delay_Ms(50)

        Send(0) = &HE8
        Send(1) = &H3
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H70
        Send(5) = &H17
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H3
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest1.Output = Send '21	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest1.Output = Send '22	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest1.Output = Send '23	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H1D
        Send(8) = &H6
        Send(9) = &H1
        Send(10) = &H3
        Send(11) = &H14
        Send(12) = &H14
        Send(13) = &HFF
        Send(14) = &H2
        Send(15) = &H0
        SerialPWBANest1.Output = Send '24	   
        Delay_Ms(50)

        Send(0) = &H4
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H1
        Send(7) = &HFF
        Send(8) = &H2C
        Send(9) = &H1
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest1.Output = Send '25	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &HFF
        Send(3) = &HFF
        Send(4) = &HFF
        Send(5) = &HFF
        Send(6) = &HFF
        Send(7) = &HFF
        Send(8) = &HFF
        Send(9) = &HFF
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest1.Output = Send '26	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &HFF
        Send(3) = &HFF
        Send(4) = &HFF
        Send(5) = &H0
        Send(6) = &HFF
        Send(7) = &HFF
        Send(8) = &HFF
        Send(9) = &HFF
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &H0
        SerialPWBANest1.Output = Send '27	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest1.Output = Send '28	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest1.Output = Send '29	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest1.Output = Send '30	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest1.Output = Send '31	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H5
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H5
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &HF4
        Send(13) = &HF4
        Send(14) = &HF4
        Send(15) = &HF4
        SerialPWBANest1.Output = Send '32	   
        Delay_Ms(50)
    End Sub

    Public Sub WriteNVMN2()
        OnComPWBAN2()

        Dim Send(15) As Byte

        Send(0) = &HFF
        Send(1) = &H0
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H1
        Send(5) = &H6
        Send(6) = &H1
        Send(7) = &H0
        Send(8) = &HF6
        Send(9) = &H0
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H27
        Send(13) = &H8D
        Send(14) = &HA
        Send(15) = &H9
        SerialPWBANest2.Output = Send '1	   
        Delay_Ms(50)

        Send(0) = &H12
        Send(1) = &HFF
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H80
        Send(5) = &H3E
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HD0
        Send(11) = &H7
        Send(12) = &HFC
        Send(13) = &H8
        Send(14) = &HA0
        Send(15) = &HF
        SerialPWBANest2.Output = Send '2	   
        Delay_Ms(50)

        Send(0) = &H94
        Send(1) = &H11
        Send(2) = &H70
        Send(3) = &H17
        Send(4) = &H2C
        Send(5) = &H1A
        Send(6) = &H40
        Send(7) = &H1F
        Send(8) = &HC4
        Send(9) = &H22
        Send(10) = &H10
        Send(11) = &H27
        Send(12) = &HC0
        Send(13) = &H2B
        Send(14) = &HE0
        Send(15) = &H2E
        SerialPWBANest2.Output = Send '3	   
        Delay_Ms(50)

        Send(0) = &H58
        Send(1) = &H34
        Send(2) = &HB0
        Send(3) = &H36
        Send(4) = &HBE
        Send(5) = &H3C
        Send(6) = &H80
        Send(7) = &H3E
        Send(8) = &H8E
        Send(9) = &H44
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest2.Output = Send '4	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H80
        Send(5) = &H3E
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HD0
        Send(11) = &H7
        Send(12) = &HFC
        Send(13) = &H8
        Send(14) = &HA0
        Send(15) = &HF
        SerialPWBANest2.Output = Send '5	   
        Delay_Ms(50)

        Send(0) = &H94
        Send(1) = &H11
        Send(2) = &H70
        Send(3) = &H17
        Send(4) = &H2C
        Send(5) = &H1A
        Send(6) = &H40
        Send(7) = &H1F
        Send(8) = &HC4
        Send(9) = &H22
        Send(10) = &H10
        Send(11) = &H27
        Send(12) = &HC0
        Send(13) = &H2B
        Send(14) = &HE0
        Send(15) = &H2E
        SerialPWBANest2.Output = Send '6	   
        Delay_Ms(50)

        Send(0) = &H58
        Send(1) = &H34
        Send(2) = &HB0
        Send(3) = &H36
        Send(4) = &HBE
        Send(5) = &H3C
        Send(6) = &H80
        Send(7) = &H3E
        Send(8) = &H8E
        Send(9) = &H44
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest2.Output = Send '7	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H80
        Send(5) = &H3E
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HD0
        Send(11) = &H7
        Send(12) = &H66
        Send(13) = &H8
        Send(14) = &HA0
        Send(15) = &HF
        SerialPWBANest2.Output = Send '8	   
        Delay_Ms(50)

        Send(0) = &HCC
        Send(1) = &H10
        Send(2) = &H70
        Send(3) = &H17
        Send(4) = &H64
        Send(5) = &H19
        Send(6) = &H40
        Send(7) = &H1F
        Send(8) = &HFC
        Send(9) = &H21
        Send(10) = &H10
        Send(11) = &H27
        Send(12) = &HF8
        Send(13) = &H2A
        Send(14) = &HE0
        Send(15) = &H2E
        SerialPWBANest2.Output = Send '9	   
        Delay_Ms(50)

        Send(0) = &H90
        Send(1) = &H33
        Send(2) = &HB0
        Send(3) = &H36
        Send(4) = &H8C
        Send(5) = &H3C
        Send(6) = &H80
        Send(7) = &H3E
        Send(8) = &H5C
        Send(9) = &H44
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest2.Output = Send '10	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &H4A
        Send(3) = &H6D
        Send(4) = &H4A
        Send(5) = &H6D
        Send(6) = &H2
        Send(7) = &H5
        Send(8) = &H32
        Send(9) = &H0
        Send(10) = &H32
        Send(11) = &H0
        Send(12) = &H4
        Send(13) = &H0
        Send(14) = &HB7
        Send(15) = &H3F
        SerialPWBANest2.Output = Send '11	   
        Delay_Ms(50)

        Send(0) = &H48
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &HFF
        Send(5) = &HFF
        Send(6) = &HFF
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HA4
        Send(11) = &H70
        Send(12) = &H3D
        Send(13) = &H0
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest2.Output = Send '12	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H19
        Send(4) = &HFF
        Send(5) = &HFF
        Send(6) = &HFF
        Send(7) = &HFF
        Send(8) = &H1
        Send(9) = &H0
        Send(10) = &H1
        Send(11) = &H0
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &H18
        Send(15) = &H3
        SerialPWBANest2.Output = Send '13	   
        Delay_Ms(50)

        Send(0) = &HE3
        Send(1) = &H0
        Send(2) = &HA
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &HFF
        Send(6) = &HE3
        Send(7) = &H0
        Send(8) = &HBE
        Send(9) = &H2
        Send(10) = &HE3
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H4C
        Send(15) = &H1
        SerialPWBANest2.Output = Send '14	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H2A
        Send(2) = &H8D
        Send(3) = &H1
        Send(4) = &H0
        Send(5) = &H55
        Send(6) = &HD2
        Send(7) = &H1
        Send(8) = &H0
        Send(9) = &H7F
        Send(10) = &H19
        Send(11) = &H2
        Send(12) = &H0
        Send(13) = &HA9
        Send(14) = &H66
        Send(15) = &H2
        SerialPWBANest2.Output = Send '15	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &HD3
        Send(2) = &HBE
        Send(3) = &H2
        Send(4) = &H0
        Send(5) = &HFE
        Send(6) = &H3F
        Send(7) = &HFF
        Send(8) = &HF2
        Send(9) = &HF2
        Send(10) = &HF2
        Send(11) = &HF2
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest2.Output = Send '16	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &H3F
        Send(7) = &H3F
        Send(8) = &H3F
        Send(9) = &H3F
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest2.Output = Send '17	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &H3F
        Send(7) = &H3F
        Send(8) = &H3F
        Send(9) = &H3F
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest2.Output = Send '18	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &H3F
        Send(7) = &H3F
        Send(8) = &H3F
        Send(9) = &H3F
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest2.Output = Send '19	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &HE8
        Send(7) = &H3
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest2.Output = Send '20	   
        Delay_Ms(50)

        Send(0) = &HE8
        Send(1) = &H3
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H70
        Send(5) = &H17
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H3
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest2.Output = Send '21	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest2.Output = Send '22	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest2.Output = Send '23	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H1D
        Send(8) = &H6
        Send(9) = &H1
        Send(10) = &H3
        Send(11) = &H14
        Send(12) = &H14
        Send(13) = &HFF
        Send(14) = &H2
        Send(15) = &H0
        SerialPWBANest2.Output = Send '24	   
        Delay_Ms(50)

        Send(0) = &H4
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H1
        Send(7) = &HFF
        Send(8) = &H2C
        Send(9) = &H1
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest2.Output = Send '25	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &HFF
        Send(3) = &HFF
        Send(4) = &HFF
        Send(5) = &HFF
        Send(6) = &HFF
        Send(7) = &HFF
        Send(8) = &HFF
        Send(9) = &HFF
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest2.Output = Send '26	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &HFF
        Send(3) = &HFF
        Send(4) = &HFF
        Send(5) = &H0
        Send(6) = &HFF
        Send(7) = &HFF
        Send(8) = &HFF
        Send(9) = &HFF
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &H0
        SerialPWBANest2.Output = Send '27	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest2.Output = Send '28	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest2.Output = Send '29	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest2.Output = Send '30	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest2.Output = Send '31	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H5
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H5
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &HF4
        Send(13) = &HF4
        Send(14) = &HF4
        Send(15) = &HF4
        SerialPWBANest2.Output = Send '32	   
        Delay_Ms(50)
    End Sub

    Public Sub WriteNVMN3()
        OnComPWBAN3()

        Dim Send(15) As Byte
        Send(0) = &HFF
        Send(1) = &H0
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H1
        Send(5) = &H6
        Send(6) = &H1
        Send(7) = &H0
        Send(8) = &HF6
        Send(9) = &H0
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H27
        Send(13) = &H8D
        Send(14) = &HA
        Send(15) = &H9
        SerialPWBANest3.Output = Send '1	   
        Delay_Ms(50)

        Send(0) = &H12
        Send(1) = &HFF
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H80
        Send(5) = &H3E
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HD0
        Send(11) = &H7
        Send(12) = &HFC
        Send(13) = &H8
        Send(14) = &HA0
        Send(15) = &HF
        SerialPWBANest3.Output = Send '2	   
        Delay_Ms(50)

        Send(0) = &H94
        Send(1) = &H11
        Send(2) = &H70
        Send(3) = &H17
        Send(4) = &H2C
        Send(5) = &H1A
        Send(6) = &H40
        Send(7) = &H1F
        Send(8) = &HC4
        Send(9) = &H22
        Send(10) = &H10
        Send(11) = &H27
        Send(12) = &HC0
        Send(13) = &H2B
        Send(14) = &HE0
        Send(15) = &H2E
        SerialPWBANest3.Output = Send '3	   
        Delay_Ms(50)

        Send(0) = &H58
        Send(1) = &H34
        Send(2) = &HB0
        Send(3) = &H36
        Send(4) = &HBE
        Send(5) = &H3C
        Send(6) = &H80
        Send(7) = &H3E
        Send(8) = &H8E
        Send(9) = &H44
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest3.Output = Send '4	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H80
        Send(5) = &H3E
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HD0
        Send(11) = &H7
        Send(12) = &HFC
        Send(13) = &H8
        Send(14) = &HA0
        Send(15) = &HF
        SerialPWBANest3.Output = Send '5	   
        Delay_Ms(50)

        Send(0) = &H94
        Send(1) = &H11
        Send(2) = &H70
        Send(3) = &H17
        Send(4) = &H2C
        Send(5) = &H1A
        Send(6) = &H40
        Send(7) = &H1F
        Send(8) = &HC4
        Send(9) = &H22
        Send(10) = &H10
        Send(11) = &H27
        Send(12) = &HC0
        Send(13) = &H2B
        Send(14) = &HE0
        Send(15) = &H2E
        SerialPWBANest3.Output = Send '6	   
        Delay_Ms(50)

        Send(0) = &H58
        Send(1) = &H34
        Send(2) = &HB0
        Send(3) = &H36
        Send(4) = &HBE
        Send(5) = &H3C
        Send(6) = &H80
        Send(7) = &H3E
        Send(8) = &H8E
        Send(9) = &H44
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest3.Output = Send '7	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H80
        Send(5) = &H3E
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HD0
        Send(11) = &H7
        Send(12) = &H66
        Send(13) = &H8
        Send(14) = &HA0
        Send(15) = &HF
        SerialPWBANest3.Output = Send '8	   
        Delay_Ms(50)

        Send(0) = &HCC
        Send(1) = &H10
        Send(2) = &H70
        Send(3) = &H17
        Send(4) = &H64
        Send(5) = &H19
        Send(6) = &H40
        Send(7) = &H1F
        Send(8) = &HFC
        Send(9) = &H21
        Send(10) = &H10
        Send(11) = &H27
        Send(12) = &HF8
        Send(13) = &H2A
        Send(14) = &HE0
        Send(15) = &H2E
        SerialPWBANest3.Output = Send '9	   
        Delay_Ms(50)

        Send(0) = &H90
        Send(1) = &H33
        Send(2) = &HB0
        Send(3) = &H36
        Send(4) = &H8C
        Send(5) = &H3C
        Send(6) = &H80
        Send(7) = &H3E
        Send(8) = &H5C
        Send(9) = &H44
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest3.Output = Send '10	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &H4A
        Send(3) = &H6D
        Send(4) = &H4A
        Send(5) = &H6D
        Send(6) = &H2
        Send(7) = &H5
        Send(8) = &H32
        Send(9) = &H0
        Send(10) = &H32
        Send(11) = &H0
        Send(12) = &H4
        Send(13) = &H0
        Send(14) = &HB7
        Send(15) = &H3F
        SerialPWBANest3.Output = Send '11	   
        Delay_Ms(50)

        Send(0) = &H48
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &HFF
        Send(5) = &HFF
        Send(6) = &HFF
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HA4
        Send(11) = &H70
        Send(12) = &H3D
        Send(13) = &H0
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest3.Output = Send '12	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H19
        Send(4) = &HFF
        Send(5) = &HFF
        Send(6) = &HFF
        Send(7) = &HFF
        Send(8) = &H1
        Send(9) = &H0
        Send(10) = &H1
        Send(11) = &H0
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &H18
        Send(15) = &H3
        SerialPWBANest3.Output = Send '13	   
        Delay_Ms(50)

        Send(0) = &HE3
        Send(1) = &H0
        Send(2) = &HA
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &HFF
        Send(6) = &HE3
        Send(7) = &H0
        Send(8) = &HBE
        Send(9) = &H2
        Send(10) = &HE3
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H4C
        Send(15) = &H1
        SerialPWBANest3.Output = Send '14	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H2A
        Send(2) = &H8D
        Send(3) = &H1
        Send(4) = &H0
        Send(5) = &H55
        Send(6) = &HD2
        Send(7) = &H1
        Send(8) = &H0
        Send(9) = &H7F
        Send(10) = &H19
        Send(11) = &H2
        Send(12) = &H0
        Send(13) = &HA9
        Send(14) = &H66
        Send(15) = &H2
        SerialPWBANest3.Output = Send '15	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &HD3
        Send(2) = &HBE
        Send(3) = &H2
        Send(4) = &H0
        Send(5) = &HFE
        Send(6) = &H3F
        Send(7) = &HFF
        Send(8) = &HF2
        Send(9) = &HF2
        Send(10) = &HF2
        Send(11) = &HF2
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest3.Output = Send '16	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &H3F
        Send(7) = &H3F
        Send(8) = &H3F
        Send(9) = &H3F
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest3.Output = Send '17	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &H3F
        Send(7) = &H3F
        Send(8) = &H3F
        Send(9) = &H3F
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest3.Output = Send '18	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &H3F
        Send(7) = &H3F
        Send(8) = &H3F
        Send(9) = &H3F
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest3.Output = Send '19	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &HE8
        Send(7) = &H3
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest3.Output = Send '20	   
        Delay_Ms(50)

        Send(0) = &HE8
        Send(1) = &H3
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H70
        Send(5) = &H17
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H3
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest3.Output = Send '21	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest3.Output = Send '22	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest3.Output = Send '23	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H1D
        Send(8) = &H6
        Send(9) = &H1
        Send(10) = &H3
        Send(11) = &H14
        Send(12) = &H14
        Send(13) = &HFF
        Send(14) = &H2
        Send(15) = &H0
        SerialPWBANest3.Output = Send '24	   
        Delay_Ms(50)

        Send(0) = &H4
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H1
        Send(7) = &HFF
        Send(8) = &H2C
        Send(9) = &H1
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest3.Output = Send '25	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &HFF
        Send(3) = &HFF
        Send(4) = &HFF
        Send(5) = &HFF
        Send(6) = &HFF
        Send(7) = &HFF
        Send(8) = &HFF
        Send(9) = &HFF
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest3.Output = Send '26	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &HFF
        Send(3) = &HFF
        Send(4) = &HFF
        Send(5) = &H0
        Send(6) = &HFF
        Send(7) = &HFF
        Send(8) = &HFF
        Send(9) = &HFF
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &H0
        SerialPWBANest3.Output = Send '27	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest3.Output = Send '28	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest3.Output = Send '29	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest3.Output = Send '30	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest3.Output = Send '31	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H5
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H5
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &HF4
        Send(13) = &HF4
        Send(14) = &HF4
        Send(15) = &HF4
        SerialPWBANest3.Output = Send '32	   
        Delay_Ms(50)
    End Sub

    Public Sub WriteNVMN4()
        OnComPWBAN4()

        Dim Send(15) As Byte

        Send(0) = &HFF
        Send(1) = &H0
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H1
        Send(5) = &H6
        Send(6) = &H1
        Send(7) = &H0
        Send(8) = &HF6
        Send(9) = &H0
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H27
        Send(13) = &H8D
        Send(14) = &HA
        Send(15) = &H9
        SerialPWBANest4.Output = Send '1	   
        Delay_Ms(50)

        Send(0) = &H12
        Send(1) = &HFF
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H80
        Send(5) = &H3E
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HD0
        Send(11) = &H7
        Send(12) = &HFC
        Send(13) = &H8
        Send(14) = &HA0
        Send(15) = &HF
        SerialPWBANest4.Output = Send '2	   
        Delay_Ms(50)

        Send(0) = &H94
        Send(1) = &H11
        Send(2) = &H70
        Send(3) = &H17
        Send(4) = &H2C
        Send(5) = &H1A
        Send(6) = &H40
        Send(7) = &H1F
        Send(8) = &HC4
        Send(9) = &H22
        Send(10) = &H10
        Send(11) = &H27
        Send(12) = &HC0
        Send(13) = &H2B
        Send(14) = &HE0
        Send(15) = &H2E
        SerialPWBANest4.Output = Send '3	   
        Delay_Ms(50)

        Send(0) = &H58
        Send(1) = &H34
        Send(2) = &HB0
        Send(3) = &H36
        Send(4) = &HBE
        Send(5) = &H3C
        Send(6) = &H80
        Send(7) = &H3E
        Send(8) = &H8E
        Send(9) = &H44
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest4.Output = Send '4	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H80
        Send(5) = &H3E
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HD0
        Send(11) = &H7
        Send(12) = &HFC
        Send(13) = &H8
        Send(14) = &HA0
        Send(15) = &HF
        SerialPWBANest4.Output = Send '5	   
        Delay_Ms(50)

        Send(0) = &H94
        Send(1) = &H11
        Send(2) = &H70
        Send(3) = &H17
        Send(4) = &H2C
        Send(5) = &H1A
        Send(6) = &H40
        Send(7) = &H1F
        Send(8) = &HC4
        Send(9) = &H22
        Send(10) = &H10
        Send(11) = &H27
        Send(12) = &HC0
        Send(13) = &H2B
        Send(14) = &HE0
        Send(15) = &H2E
        SerialPWBANest4.Output = Send '6	   
        Delay_Ms(50)

        Send(0) = &H58
        Send(1) = &H34
        Send(2) = &HB0
        Send(3) = &H36
        Send(4) = &HBE
        Send(5) = &H3C
        Send(6) = &H80
        Send(7) = &H3E
        Send(8) = &H8E
        Send(9) = &H44
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest4.Output = Send '7	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H80
        Send(5) = &H3E
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HD0
        Send(11) = &H7
        Send(12) = &H66
        Send(13) = &H8
        Send(14) = &HA0
        Send(15) = &HF
        SerialPWBANest4.Output = Send '8	   
        Delay_Ms(50)

        Send(0) = &HCC
        Send(1) = &H10
        Send(2) = &H70
        Send(3) = &H17
        Send(4) = &H64
        Send(5) = &H19
        Send(6) = &H40
        Send(7) = &H1F
        Send(8) = &HFC
        Send(9) = &H21
        Send(10) = &H10
        Send(11) = &H27
        Send(12) = &HF8
        Send(13) = &H2A
        Send(14) = &HE0
        Send(15) = &H2E
        SerialPWBANest4.Output = Send '9	   
        Delay_Ms(50)

        Send(0) = &H90
        Send(1) = &H33
        Send(2) = &HB0
        Send(3) = &H36
        Send(4) = &H8C
        Send(5) = &H3C
        Send(6) = &H80
        Send(7) = &H3E
        Send(8) = &H5C
        Send(9) = &H44
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest4.Output = Send '10	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &H4A
        Send(3) = &H6D
        Send(4) = &H4A
        Send(5) = &H6D
        Send(6) = &H2
        Send(7) = &H5
        Send(8) = &H32
        Send(9) = &H0
        Send(10) = &H32
        Send(11) = &H0
        Send(12) = &H4
        Send(13) = &H0
        Send(14) = &HB7
        Send(15) = &H3F
        SerialPWBANest4.Output = Send '11	   
        Delay_Ms(50)

        Send(0) = &H48
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &HFF
        Send(5) = &HFF
        Send(6) = &HFF
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &HA4
        Send(11) = &H70
        Send(12) = &H3D
        Send(13) = &H0
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest4.Output = Send '12	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H19
        Send(4) = &HFF
        Send(5) = &HFF
        Send(6) = &HFF
        Send(7) = &HFF
        Send(8) = &H1
        Send(9) = &H0
        Send(10) = &H1
        Send(11) = &H0
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &H18
        Send(15) = &H3
        SerialPWBANest4.Output = Send '13	   
        Delay_Ms(50)

        Send(0) = &HE3
        Send(1) = &H0
        Send(2) = &HA
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &HFF
        Send(6) = &HE3
        Send(7) = &H0
        Send(8) = &HBE
        Send(9) = &H2
        Send(10) = &HE3
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H4C
        Send(15) = &H1
        SerialPWBANest4.Output = Send '14	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H2A
        Send(2) = &H8D
        Send(3) = &H1
        Send(4) = &H0
        Send(5) = &H55
        Send(6) = &HD2
        Send(7) = &H1
        Send(8) = &H0
        Send(9) = &H7F
        Send(10) = &H19
        Send(11) = &H2
        Send(12) = &H0
        Send(13) = &HA9
        Send(14) = &H66
        Send(15) = &H2
        SerialPWBANest4.Output = Send '15	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &HD3
        Send(2) = &HBE
        Send(3) = &H2
        Send(4) = &H0
        Send(5) = &HFE
        Send(6) = &H3F
        Send(7) = &HFF
        Send(8) = &HF2
        Send(9) = &HF2
        Send(10) = &HF2
        Send(11) = &HF2
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest4.Output = Send '16	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &H3F
        Send(7) = &H3F
        Send(8) = &H3F
        Send(9) = &H3F
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest4.Output = Send '17	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &H3F
        Send(7) = &H3F
        Send(8) = &H3F
        Send(9) = &H3F
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest4.Output = Send '18	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &H3F
        Send(7) = &H3F
        Send(8) = &H3F
        Send(9) = &H3F
        Send(10) = &H3F
        Send(11) = &H3F
        Send(12) = &H3F
        Send(13) = &H3F
        Send(14) = &H3F
        Send(15) = &H3F
        SerialPWBANest4.Output = Send '19	   
        Delay_Ms(50)

        Send(0) = &H3F
        Send(1) = &H3F
        Send(2) = &H3F
        Send(3) = &H3F
        Send(4) = &H3F
        Send(5) = &H3F
        Send(6) = &HE8
        Send(7) = &H3
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest4.Output = Send '20	   
        Delay_Ms(50)

        Send(0) = &HE8
        Send(1) = &H3
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H70
        Send(5) = &H17
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H3
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest4.Output = Send '21	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest4.Output = Send '22	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H0
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &H0
        Send(14) = &H0
        Send(15) = &H0
        SerialPWBANest4.Output = Send '23	   
        Delay_Ms(50)

        Send(0) = &H0
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H1D
        Send(8) = &H6
        Send(9) = &H1
        Send(10) = &H3
        Send(11) = &H14
        Send(12) = &H14
        Send(13) = &HFF
        Send(14) = &H2
        Send(15) = &H0
        SerialPWBANest4.Output = Send '24	   
        Delay_Ms(50)

        Send(0) = &H4
        Send(1) = &H0
        Send(2) = &H0
        Send(3) = &H0
        Send(4) = &H0
        Send(5) = &H0
        Send(6) = &H1
        Send(7) = &HFF
        Send(8) = &H2C
        Send(9) = &H1
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &H0
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest4.Output = Send '25	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &HFF
        Send(3) = &HFF
        Send(4) = &HFF
        Send(5) = &HFF
        Send(6) = &HFF
        Send(7) = &HFF
        Send(8) = &HFF
        Send(9) = &HFF
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &HFF
        SerialPWBANest4.Output = Send '26	   
        Delay_Ms(50)

        Send(0) = &HFF
        Send(1) = &HFF
        Send(2) = &HFF
        Send(3) = &HFF
        Send(4) = &HFF
        Send(5) = &H0
        Send(6) = &HFF
        Send(7) = &HFF
        Send(8) = &HFF
        Send(9) = &HFF
        Send(10) = &HFF
        Send(11) = &HFF
        Send(12) = &HFF
        Send(13) = &HFF
        Send(14) = &HFF
        Send(15) = &H0
        SerialPWBANest4.Output = Send '27	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest4.Output = Send '28	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest4.Output = Send '29	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest4.Output = Send '30	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H6
        Send(5) = &H0
        Send(6) = &HFE
        Send(7) = &H0
        Send(8) = &H6
        Send(9) = &H0
        Send(10) = &HFE
        Send(11) = &H0
        Send(12) = &H6
        Send(13) = &H0
        Send(14) = &HFE
        Send(15) = &H0
        SerialPWBANest4.Output = Send '31	   
        Delay_Ms(50)

        Send(0) = &H6
        Send(1) = &H0
        Send(2) = &HFE
        Send(3) = &H0
        Send(4) = &H5
        Send(5) = &H0
        Send(6) = &H0
        Send(7) = &H0
        Send(8) = &H5
        Send(9) = &H0
        Send(10) = &H0
        Send(11) = &H0
        Send(12) = &HF4
        Send(13) = &HF4
        Send(14) = &HF4
        Send(15) = &HF4
        SerialPWBANest4.Output = Send '32	   
        Delay_Ms(50)
    End Sub

    Public Sub CheckNVMDataN1()
        If strhexbuffN1(0) = "FF" And
strhexbuffN1(1) = "00" And
strhexbuffN1(2) = "3F" And
strhexbuffN1(3) = "3F" And
strhexbuffN1(4) = "01" And
strhexbuffN1(5) = "06" And
strhexbuffN1(6) = "01" And
strhexbuffN1(7) = "00" And
strhexbuffN1(8) = "F6" And
strhexbuffN1(9) = "00" And
strhexbuffN1(10) = "3F" And
strhexbuffN1(11) = "3F" And
strhexbuffN1(12) = "27" And
strhexbuffN1(13) = "8D" And
strhexbuffN1(14) = "0A" And
strhexbuffN1(15) = "09" And
strhexbuffN1(16) = "12" And
strhexbuffN1(17) = "FF" And
strhexbuffN1(18) = "00" And
strhexbuffN1(19) = "00" And
strhexbuffN1(20) = "80" And
strhexbuffN1(21) = "3E" And
strhexbuffN1(22) = "00" And
strhexbuffN1(23) = "00" And
strhexbuffN1(24) = "00" And
strhexbuffN1(25) = "00" And
strhexbuffN1(26) = "D0" And
strhexbuffN1(27) = "07" And
strhexbuffN1(28) = "FC" And
strhexbuffN1(29) = "08" And
strhexbuffN1(30) = "A0" And
strhexbuffN1(31) = "0F" And
strhexbuffN1(32) = "94" And
strhexbuffN1(33) = "11" And
strhexbuffN1(34) = "70" And
strhexbuffN1(35) = "17" And
strhexbuffN1(36) = "2C" And
strhexbuffN1(37) = "1A" And
strhexbuffN1(38) = "40" And
strhexbuffN1(39) = "1F" And
strhexbuffN1(40) = "C4" And
strhexbuffN1(41) = "22" And
strhexbuffN1(42) = "10" And
strhexbuffN1(43) = "27" And
strhexbuffN1(44) = "C0" And
strhexbuffN1(45) = "2B" And
strhexbuffN1(46) = "E0" And
strhexbuffN1(47) = "2E" And
strhexbuffN1(48) = "58" And
strhexbuffN1(49) = "34" And
strhexbuffN1(50) = "B0" And
strhexbuffN1(51) = "36" And
strhexbuffN1(52) = "BE" And
strhexbuffN1(53) = "3C" And
strhexbuffN1(54) = "80" And
strhexbuffN1(55) = "3E" And
strhexbuffN1(56) = "8E" And
strhexbuffN1(57) = "44" And
strhexbuffN1(58) = "FF" And
strhexbuffN1(59) = "FF" And
strhexbuffN1(60) = "FF" And
strhexbuffN1(61) = "FF" And
strhexbuffN1(62) = "FF" And
strhexbuffN1(63) = "FF" And
strhexbuffN1(64) = "FF" And
strhexbuffN1(65) = "FF" And
strhexbuffN1(66) = "00" And
strhexbuffN1(67) = "00" And
strhexbuffN1(68) = "80" And
strhexbuffN1(69) = "3E" And
strhexbuffN1(70) = "00" And
strhexbuffN1(71) = "00" And
strhexbuffN1(72) = "00" And
strhexbuffN1(73) = "00" And
strhexbuffN1(74) = "D0" And
strhexbuffN1(75) = "07" And
strhexbuffN1(76) = "FC" And
strhexbuffN1(77) = "08" And
strhexbuffN1(78) = "A0" And
strhexbuffN1(79) = "0F" And
strhexbuffN1(80) = "94" And
strhexbuffN1(81) = "11" And
strhexbuffN1(82) = "70" And
strhexbuffN1(83) = "17" And
strhexbuffN1(84) = "2C" And
strhexbuffN1(85) = "1A" And
strhexbuffN1(86) = "40" And
strhexbuffN1(87) = "1F" And
strhexbuffN1(88) = "C4" And
strhexbuffN1(89) = "22" And
strhexbuffN1(90) = "10" And
strhexbuffN1(91) = "27" And
strhexbuffN1(92) = "C0" And
strhexbuffN1(93) = "2B" And
strhexbuffN1(94) = "E0" And
strhexbuffN1(95) = "2E" And
strhexbuffN1(96) = "58" And
strhexbuffN1(97) = "34" And
strhexbuffN1(98) = "B0" And
strhexbuffN1(99) = "36" And
strhexbuffN1(100) = "BE" And
strhexbuffN1(101) = "3C" And
strhexbuffN1(102) = "80" And
strhexbuffN1(103) = "3E" And
strhexbuffN1(104) = "8E" And
strhexbuffN1(105) = "44" And
strhexbuffN1(106) = "FF" And
strhexbuffN1(107) = "FF" And
strhexbuffN1(108) = "FF" And
strhexbuffN1(109) = "FF" And
strhexbuffN1(110) = "FF" And
strhexbuffN1(111) = "FF" And
strhexbuffN1(112) = "FF" And
strhexbuffN1(113) = "FF" And
strhexbuffN1(114) = "00" And
strhexbuffN1(115) = "00" And
strhexbuffN1(116) = "80" And
strhexbuffN1(117) = "3E" And
strhexbuffN1(118) = "00" And
strhexbuffN1(119) = "00" And
strhexbuffN1(120) = "00" And
strhexbuffN1(121) = "00" And
strhexbuffN1(122) = "D0" And
strhexbuffN1(123) = "07" And
strhexbuffN1(124) = "66" And
strhexbuffN1(125) = "08" And
strhexbuffN1(126) = "A0" And
strhexbuffN1(127) = "0F" And
strhexbuffN1(128) = "CC" And
strhexbuffN1(129) = "10" And
strhexbuffN1(130) = "70" And
strhexbuffN1(131) = "17" And
strhexbuffN1(132) = "64" And
strhexbuffN1(133) = "19" And
strhexbuffN1(134) = "40" And
strhexbuffN1(135) = "1F" And
strhexbuffN1(136) = "FC" And
strhexbuffN1(137) = "21" And
strhexbuffN1(138) = "10" And
strhexbuffN1(139) = "27" And
strhexbuffN1(140) = "F8" And
strhexbuffN1(141) = "2A" And
strhexbuffN1(142) = "E0" And
strhexbuffN1(143) = "2E" And
strhexbuffN1(144) = "90" And
strhexbuffN1(145) = "33" And
strhexbuffN1(146) = "B0" And
strhexbuffN1(147) = "36" And
strhexbuffN1(148) = "8C" And
strhexbuffN1(149) = "3C" And
strhexbuffN1(150) = "80" And
strhexbuffN1(151) = "3E" And
strhexbuffN1(152) = "5C" And
strhexbuffN1(153) = "44" And
strhexbuffN1(154) = "FF" And
strhexbuffN1(155) = "FF" And
strhexbuffN1(156) = "FF" And
strhexbuffN1(157) = "FF" And
strhexbuffN1(158) = "FF" And
strhexbuffN1(159) = "FF" And
strhexbuffN1(160) = "FF" And
strhexbuffN1(161) = "FF" And
strhexbuffN1(162) = "4A" And
strhexbuffN1(163) = "6D" And
strhexbuffN1(164) = "4A" And
strhexbuffN1(165) = "6D" And
strhexbuffN1(166) = "02" And
strhexbuffN1(167) = "05" And
strhexbuffN1(168) = "32" And
strhexbuffN1(169) = "00" And
strhexbuffN1(170) = "32" And
strhexbuffN1(171) = "00" And
strhexbuffN1(172) = "04" And
strhexbuffN1(173) = "00" And
strhexbuffN1(174) = "B7" And
strhexbuffN1(175) = "3F" And
strhexbuffN1(176) = "48" And
strhexbuffN1(177) = "00" And
strhexbuffN1(178) = "00" And
strhexbuffN1(179) = "00" And
strhexbuffN1(180) = "FF" And
strhexbuffN1(181) = "FF" And
strhexbuffN1(182) = "FF" And
strhexbuffN1(183) = "00" And
strhexbuffN1(184) = "00" And
strhexbuffN1(185) = "00" And
strhexbuffN1(186) = "A4" And
strhexbuffN1(187) = "70" And
strhexbuffN1(188) = "3D" And
strhexbuffN1(189) = "00" And
strhexbuffN1(190) = "FF" And
strhexbuffN1(191) = "FF" And
strhexbuffN1(192) = "FF" And
strhexbuffN1(193) = "00" And
strhexbuffN1(194) = "00" And
strhexbuffN1(195) = "19" And
strhexbuffN1(196) = "FF" And
strhexbuffN1(197) = "FF" And
strhexbuffN1(198) = "FF" And
strhexbuffN1(199) = "FF" And
strhexbuffN1(200) = "01" And
strhexbuffN1(201) = "00" And
strhexbuffN1(202) = "01" And
strhexbuffN1(203) = "00" And
strhexbuffN1(204) = "FF" And
strhexbuffN1(205) = "FF" And
strhexbuffN1(206) = "18" And
strhexbuffN1(207) = "03" And
strhexbuffN1(208) = "E3" And
strhexbuffN1(209) = "00" And
strhexbuffN1(210) = "0A" And
strhexbuffN1(211) = "00" And
strhexbuffN1(212) = "00" And
strhexbuffN1(213) = "FF" And
strhexbuffN1(214) = "E3" And
strhexbuffN1(215) = "00" And
strhexbuffN1(216) = "BE" And
strhexbuffN1(217) = "02" And
strhexbuffN1(218) = "E3" And
strhexbuffN1(219) = "00" And
strhexbuffN1(220) = "00" And
strhexbuffN1(221) = "00" And
strhexbuffN1(222) = "4C" And
strhexbuffN1(223) = "01" And
strhexbuffN1(224) = "00" And
strhexbuffN1(225) = "2A" And
strhexbuffN1(226) = "8D" And
strhexbuffN1(227) = "01" And
strhexbuffN1(228) = "00" And
strhexbuffN1(229) = "55" And
strhexbuffN1(230) = "D2" And
strhexbuffN1(231) = "01" And
strhexbuffN1(232) = "00" And
strhexbuffN1(233) = "7F" And
strhexbuffN1(234) = "19" And
strhexbuffN1(235) = "02" And
strhexbuffN1(236) = "00" And
strhexbuffN1(237) = "A9" And
strhexbuffN1(238) = "66" And
strhexbuffN1(239) = "02" And
strhexbuffN1(240) = "00" And
strhexbuffN1(241) = "D3" And
strhexbuffN1(242) = "BE" And
strhexbuffN1(243) = "02" And
strhexbuffN1(244) = "00" And
strhexbuffN1(245) = "FE" And
strhexbuffN1(246) = "3F" And
strhexbuffN1(247) = "FF" And
strhexbuffN1(248) = "F2" And
strhexbuffN1(249) = "F2" And
strhexbuffN1(250) = "F2" And
strhexbuffN1(251) = "F2" And
strhexbuffN1(252) = "3F" And
strhexbuffN1(253) = "3F" And
strhexbuffN1(254) = "3F" And
strhexbuffN1(255) = "3F" And
strhexbuffN1(256) = "3F" And
strhexbuffN1(257) = "3F" And
strhexbuffN1(258) = "3F" And
strhexbuffN1(259) = "3F" And
strhexbuffN1(260) = "3F" And
strhexbuffN1(261) = "3F" And
strhexbuffN1(262) = "3F" And
strhexbuffN1(263) = "3F" And
strhexbuffN1(264) = "3F" And
strhexbuffN1(265) = "3F" And
strhexbuffN1(266) = "3F" And
strhexbuffN1(267) = "3F" And
strhexbuffN1(268) = "3F" And
strhexbuffN1(269) = "3F" And
strhexbuffN1(270) = "3F" And
strhexbuffN1(271) = "3F" And
strhexbuffN1(272) = "3F" And
strhexbuffN1(273) = "3F" And
strhexbuffN1(274) = "3F" And
strhexbuffN1(275) = "3F" And
strhexbuffN1(276) = "3F" And
strhexbuffN1(277) = "3F" And
strhexbuffN1(278) = "3F" And
strhexbuffN1(279) = "3F" And
strhexbuffN1(280) = "3F" And
strhexbuffN1(281) = "3F" And
strhexbuffN1(282) = "3F" And
strhexbuffN1(283) = "3F" And
strhexbuffN1(284) = "3F" And
strhexbuffN1(285) = "3F" And
strhexbuffN1(286) = "3F" And
strhexbuffN1(287) = "3F" And
strhexbuffN1(288) = "3F" And
strhexbuffN1(289) = "3F" And
strhexbuffN1(290) = "3F" And
strhexbuffN1(291) = "3F" And
strhexbuffN1(292) = "3F" And
strhexbuffN1(293) = "3F" And
strhexbuffN1(294) = "3F" And
strhexbuffN1(295) = "3F" And
strhexbuffN1(296) = "3F" And
strhexbuffN1(297) = "3F" And
strhexbuffN1(298) = "3F" And
strhexbuffN1(299) = "3F" And
strhexbuffN1(300) = "3F" And
strhexbuffN1(301) = "3F" And
strhexbuffN1(302) = "3F" And
strhexbuffN1(303) = "3F" And
strhexbuffN1(304) = "3F" And
strhexbuffN1(305) = "3F" And
strhexbuffN1(306) = "3F" And
strhexbuffN1(307) = "3F" And
strhexbuffN1(308) = "3F" And
strhexbuffN1(309) = "3F" And
strhexbuffN1(310) = "E8" And
strhexbuffN1(311) = "03" And
strhexbuffN1(312) = "00" And
strhexbuffN1(313) = "00" And
strhexbuffN1(314) = "00" And
strhexbuffN1(315) = "00" And
strhexbuffN1(316) = "00" And
strhexbuffN1(317) = "00" And
strhexbuffN1(318) = "00" And
strhexbuffN1(319) = "00" And
strhexbuffN1(320) = "E8" And
strhexbuffN1(321) = "03" And
strhexbuffN1(322) = "00" And
strhexbuffN1(323) = "00" And
strhexbuffN1(324) = "70" And
strhexbuffN1(325) = "17" And
strhexbuffN1(326) = "00" And
strhexbuffN1(327) = "00" And
strhexbuffN1(328) = "03" And
strhexbuffN1(329) = "00" And
strhexbuffN1(330) = "00" And
strhexbuffN1(331) = "00" And
strhexbuffN1(332) = "00" And
strhexbuffN1(333) = "00" And
strhexbuffN1(334) = "00" And
strhexbuffN1(335) = "00" And
strhexbuffN1(336) = "00" And
strhexbuffN1(337) = "00" And
strhexbuffN1(338) = "00" And
strhexbuffN1(339) = "00" And
strhexbuffN1(340) = "00" And
strhexbuffN1(341) = "00" And
strhexbuffN1(342) = "00" And
strhexbuffN1(343) = "00" And
strhexbuffN1(344) = "00" And
strhexbuffN1(345) = "00" And
strhexbuffN1(346) = "00" And
strhexbuffN1(347) = "00" And
strhexbuffN1(348) = "00" And
strhexbuffN1(349) = "00" And
strhexbuffN1(350) = "00" And
strhexbuffN1(351) = "00" And
strhexbuffN1(352) = "00" And
strhexbuffN1(353) = "00" And
strhexbuffN1(354) = "00" And
strhexbuffN1(355) = "00" And
strhexbuffN1(356) = "00" And
strhexbuffN1(357) = "00" And
strhexbuffN1(358) = "00" And
strhexbuffN1(359) = "00" And
strhexbuffN1(360) = "00" And
strhexbuffN1(361) = "00" And
strhexbuffN1(362) = "00" And
strhexbuffN1(363) = "00" And
strhexbuffN1(364) = "00" And
strhexbuffN1(365) = "00" And
strhexbuffN1(366) = "00" And
strhexbuffN1(367) = "00" And
strhexbuffN1(368) = "00" And
strhexbuffN1(369) = "00" And
strhexbuffN1(370) = "00" And
strhexbuffN1(371) = "00" And
strhexbuffN1(372) = "00" And
strhexbuffN1(373) = "00" And
strhexbuffN1(374) = "00" And
strhexbuffN1(375) = "1D" And
strhexbuffN1(376) = "06" And
strhexbuffN1(377) = "01" And
strhexbuffN1(378) = "03" And
strhexbuffN1(379) = "14" And
strhexbuffN1(380) = "14" And
strhexbuffN1(381) = "FF" And
strhexbuffN1(382) = "02" And
strhexbuffN1(383) = "00" And
strhexbuffN1(384) = "04" And
strhexbuffN1(385) = "00" And
strhexbuffN1(386) = "00" And
strhexbuffN1(387) = "00" And
strhexbuffN1(388) = "00" And
strhexbuffN1(389) = "00" And
strhexbuffN1(390) = "01" And
strhexbuffN1(391) = "FF" And
strhexbuffN1(392) = "2C" And
strhexbuffN1(393) = "01" And
strhexbuffN1(394) = "00" And
strhexbuffN1(395) = "00" And
strhexbuffN1(396) = "00" And
strhexbuffN1(397) = "FF" And
strhexbuffN1(398) = "FF" And
strhexbuffN1(399) = "FF" And
strhexbuffN1(400) = "FF" And
strhexbuffN1(401) = "FF" And
strhexbuffN1(402) = "FF" And
strhexbuffN1(403) = "FF" And
strhexbuffN1(404) = "FF" And
strhexbuffN1(405) = "FF" And
strhexbuffN1(406) = "FF" And
strhexbuffN1(407) = "FF" And
strhexbuffN1(408) = "FF" And
strhexbuffN1(409) = "FF" And
strhexbuffN1(410) = "FF" And
strhexbuffN1(411) = "FF" And
strhexbuffN1(412) = "FF" And
strhexbuffN1(413) = "FF" And
strhexbuffN1(414) = "FF" And
strhexbuffN1(415) = "FF" And
strhexbuffN1(416) = "FF" And
strhexbuffN1(417) = "FF" And
strhexbuffN1(418) = "FF" And
strhexbuffN1(419) = "FF" And
strhexbuffN1(420) = "FF" And
strhexbuffN1(421) = "00" And
strhexbuffN1(422) = "FF" And
strhexbuffN1(423) = "FF" And
strhexbuffN1(424) = "FF" And
strhexbuffN1(425) = "FF" And
strhexbuffN1(426) = "FF" And
strhexbuffN1(427) = "FF" And
strhexbuffN1(428) = "FF" And
strhexbuffN1(429) = "FF" And
strhexbuffN1(430) = "FF" And
strhexbuffN1(431) = "00" And
strhexbuffN1(432) = "06" And
strhexbuffN1(433) = "00" And
strhexbuffN1(434) = "FE" And
strhexbuffN1(435) = "00" And
strhexbuffN1(436) = "06" And
strhexbuffN1(437) = "00" And
strhexbuffN1(438) = "FE" And
strhexbuffN1(439) = "00" And
strhexbuffN1(440) = "06" And
strhexbuffN1(441) = "00" And
strhexbuffN1(442) = "FE" And
strhexbuffN1(443) = "00" And
strhexbuffN1(444) = "06" And
strhexbuffN1(445) = "00" And
strhexbuffN1(446) = "FE" And
strhexbuffN1(447) = "00" And
strhexbuffN1(448) = "06" And
strhexbuffN1(449) = "00" And
strhexbuffN1(450) = "FE" And
strhexbuffN1(451) = "00" And
strhexbuffN1(452) = "06" And
strhexbuffN1(453) = "00" And
strhexbuffN1(454) = "FE" And
strhexbuffN1(455) = "00" And
strhexbuffN1(456) = "06" And
strhexbuffN1(457) = "00" And
strhexbuffN1(458) = "FE" And
strhexbuffN1(459) = "00" And
strhexbuffN1(460) = "06" And
strhexbuffN1(461) = "00" And
strhexbuffN1(462) = "FE" And
strhexbuffN1(463) = "00" And
strhexbuffN1(464) = "06" And
strhexbuffN1(465) = "00" And
strhexbuffN1(466) = "FE" And
strhexbuffN1(467) = "00" And
strhexbuffN1(468) = "06" And
strhexbuffN1(469) = "00" And
strhexbuffN1(470) = "FE" And
strhexbuffN1(471) = "00" And
strhexbuffN1(472) = "06" And
strhexbuffN1(473) = "00" And
strhexbuffN1(474) = "FE" And
strhexbuffN1(475) = "00" And
strhexbuffN1(476) = "06" And
strhexbuffN1(477) = "00" And
strhexbuffN1(478) = "FE" And
strhexbuffN1(479) = "00" And
strhexbuffN1(480) = "06" And
strhexbuffN1(481) = "00" And
strhexbuffN1(482) = "FE" And
strhexbuffN1(483) = "00" And
strhexbuffN1(484) = "06" And
strhexbuffN1(485) = "00" And
strhexbuffN1(486) = "FE" And
strhexbuffN1(487) = "00" And
strhexbuffN1(488) = "06" And
strhexbuffN1(489) = "00" And
strhexbuffN1(490) = "FE" And
strhexbuffN1(491) = "00" And
strhexbuffN1(492) = "06" And
strhexbuffN1(493) = "00" And
strhexbuffN1(494) = "FE" And
strhexbuffN1(495) = "00" And
            strhexbuffN1(496) = "06" And
strhexbuffN1(497) = "00" And
strhexbuffN1(498) = "FE" And
strhexbuffN1(499) = "00" And
strhexbuffN1(500) = "05" And
strhexbuffN1(501) = "00" And
strhexbuffN1(502) = "00" And
strhexbuffN1(503) = "00" And
strhexbuffN1(504) = "05" And
strhexbuffN1(505) = "00" And
strhexbuffN1(506) = "00" And
strhexbuffN1(507) = "00" And
strhexbuffN1(508) = "F4" And
strhexbuffN1(509) = "F4" And
strhexbuffN1(510) = "F4" And
strhexbuffN1(511) = "F4" Then
            ValidateNVMN1 = True
        Else
            ValidateNVMN1 = False
        End If

    End Sub

    Public Sub CheckNVMDataN2()
        If strhexbuffN2(0) = "FF" And
strhexbuffN2(1) = "00" And
strhexbuffN2(2) = "3F" And
strhexbuffN2(3) = "3F" And
strhexbuffN2(4) = "01" And
strhexbuffN2(5) = "06" And
strhexbuffN2(6) = "01" And
strhexbuffN2(7) = "00" And
strhexbuffN2(8) = "F6" And
strhexbuffN2(9) = "00" And
strhexbuffN2(10) = "3F" And
strhexbuffN2(11) = "3F" And
strhexbuffN2(12) = "27" And
strhexbuffN2(13) = "8D" And
strhexbuffN2(14) = "0A" And
strhexbuffN2(15) = "09" And
strhexbuffN2(16) = "12" And
strhexbuffN2(17) = "FF" And
strhexbuffN2(18) = "00" And
strhexbuffN2(19) = "00" And
strhexbuffN2(20) = "80" And
strhexbuffN2(21) = "3E" And
strhexbuffN2(22) = "00" And
strhexbuffN2(23) = "00" And
strhexbuffN2(24) = "00" And
strhexbuffN2(25) = "00" And
strhexbuffN2(26) = "D0" And
strhexbuffN2(27) = "07" And
strhexbuffN2(28) = "FC" And
strhexbuffN2(29) = "08" And
strhexbuffN2(30) = "A0" And
strhexbuffN2(31) = "0F" And
strhexbuffN2(32) = "94" And
strhexbuffN2(33) = "11" And
strhexbuffN2(34) = "70" And
strhexbuffN2(35) = "17" And
strhexbuffN2(36) = "2C" And
strhexbuffN2(37) = "1A" And
strhexbuffN2(38) = "40" And
strhexbuffN2(39) = "1F" And
strhexbuffN2(40) = "C4" And
strhexbuffN2(41) = "22" And
strhexbuffN2(42) = "10" And
strhexbuffN2(43) = "27" And
strhexbuffN2(44) = "C0" And
strhexbuffN2(45) = "2B" And
strhexbuffN2(46) = "E0" And
strhexbuffN2(47) = "2E" And
strhexbuffN2(48) = "58" And
strhexbuffN2(49) = "34" And
strhexbuffN2(50) = "B0" And
strhexbuffN2(51) = "36" And
strhexbuffN2(52) = "BE" And
strhexbuffN2(53) = "3C" And
strhexbuffN2(54) = "80" And
strhexbuffN2(55) = "3E" And
strhexbuffN2(56) = "8E" And
strhexbuffN2(57) = "44" And
strhexbuffN2(58) = "FF" And
strhexbuffN2(59) = "FF" And
strhexbuffN2(60) = "FF" And
strhexbuffN2(61) = "FF" And
strhexbuffN2(62) = "FF" And
strhexbuffN2(63) = "FF" And
strhexbuffN2(64) = "FF" And
strhexbuffN2(65) = "FF" And
strhexbuffN2(66) = "00" And
strhexbuffN2(67) = "00" And
strhexbuffN2(68) = "80" And
strhexbuffN2(69) = "3E" And
strhexbuffN2(70) = "00" And
strhexbuffN2(71) = "00" And
strhexbuffN2(72) = "00" And
strhexbuffN2(73) = "00" And
strhexbuffN2(74) = "D0" And
strhexbuffN2(75) = "07" And
strhexbuffN2(76) = "FC" And
strhexbuffN2(77) = "08" And
strhexbuffN2(78) = "A0" And
strhexbuffN2(79) = "0F" And
strhexbuffN2(80) = "94" And
strhexbuffN2(81) = "11" And
strhexbuffN2(82) = "70" And
strhexbuffN2(83) = "17" And
strhexbuffN2(84) = "2C" And
strhexbuffN2(85) = "1A" And
strhexbuffN2(86) = "40" And
strhexbuffN2(87) = "1F" And
strhexbuffN2(88) = "C4" And
strhexbuffN2(89) = "22" And
strhexbuffN2(90) = "10" And
strhexbuffN2(91) = "27" And
strhexbuffN2(92) = "C0" And
strhexbuffN2(93) = "2B" And
strhexbuffN2(94) = "E0" And
strhexbuffN2(95) = "2E" And
strhexbuffN2(96) = "58" And
strhexbuffN2(97) = "34" And
strhexbuffN2(98) = "B0" And
strhexbuffN2(99) = "36" And
strhexbuffN2(100) = "BE" And
strhexbuffN2(101) = "3C" And
strhexbuffN2(102) = "80" And
strhexbuffN2(103) = "3E" And
strhexbuffN2(104) = "8E" And
strhexbuffN2(105) = "44" And
strhexbuffN2(106) = "FF" And
strhexbuffN2(107) = "FF" And
strhexbuffN2(108) = "FF" And
strhexbuffN2(109) = "FF" And
strhexbuffN2(110) = "FF" And
strhexbuffN2(111) = "FF" And
strhexbuffN2(112) = "FF" And
strhexbuffN2(113) = "FF" And
strhexbuffN2(114) = "00" And
strhexbuffN2(115) = "00" And
strhexbuffN2(116) = "80" And
strhexbuffN2(117) = "3E" And
strhexbuffN2(118) = "00" And
strhexbuffN2(119) = "00" And
strhexbuffN2(120) = "00" And
strhexbuffN2(121) = "00" And
strhexbuffN2(122) = "D0" And
strhexbuffN2(123) = "07" And
strhexbuffN2(124) = "66" And
strhexbuffN2(125) = "08" And
strhexbuffN2(126) = "A0" And
strhexbuffN2(127) = "0F" And
strhexbuffN2(128) = "CC" And
strhexbuffN2(129) = "10" And
strhexbuffN2(130) = "70" And
strhexbuffN2(131) = "17" And
strhexbuffN2(132) = "64" And
strhexbuffN2(133) = "19" And
strhexbuffN2(134) = "40" And
strhexbuffN2(135) = "1F" And
strhexbuffN2(136) = "FC" And
strhexbuffN2(137) = "21" And
strhexbuffN2(138) = "10" And
strhexbuffN2(139) = "27" And
strhexbuffN2(140) = "F8" And
strhexbuffN2(141) = "2A" And
strhexbuffN2(142) = "E0" And
strhexbuffN2(143) = "2E" And
strhexbuffN2(144) = "90" And
strhexbuffN2(145) = "33" And
strhexbuffN2(146) = "B0" And
strhexbuffN2(147) = "36" And
strhexbuffN2(148) = "8C" And
strhexbuffN2(149) = "3C" And
strhexbuffN2(150) = "80" And
strhexbuffN2(151) = "3E" And
strhexbuffN2(152) = "5C" And
strhexbuffN2(153) = "44" And
strhexbuffN2(154) = "FF" And
strhexbuffN2(155) = "FF" And
strhexbuffN2(156) = "FF" And
strhexbuffN2(157) = "FF" And
strhexbuffN2(158) = "FF" And
strhexbuffN2(159) = "FF" And
strhexbuffN2(160) = "FF" And
strhexbuffN2(161) = "FF" And
strhexbuffN2(162) = "4A" And
strhexbuffN2(163) = "6D" And
strhexbuffN2(164) = "4A" And
strhexbuffN2(165) = "6D" And
strhexbuffN2(166) = "02" And
strhexbuffN2(167) = "05" And
strhexbuffN2(168) = "32" And
strhexbuffN2(169) = "00" And
strhexbuffN2(170) = "32" And
strhexbuffN2(171) = "00" And
strhexbuffN2(172) = "04" And
strhexbuffN2(173) = "00" And
strhexbuffN2(174) = "B7" And
strhexbuffN2(175) = "3F" And
strhexbuffN2(176) = "48" And
strhexbuffN2(177) = "00" And
strhexbuffN2(178) = "00" And
strhexbuffN2(179) = "00" And
strhexbuffN2(180) = "FF" And
strhexbuffN2(181) = "FF" And
strhexbuffN2(182) = "FF" And
strhexbuffN2(183) = "00" And
strhexbuffN2(184) = "00" And
strhexbuffN2(185) = "00" And
strhexbuffN2(186) = "A4" And
strhexbuffN2(187) = "70" And
strhexbuffN2(188) = "3D" And
strhexbuffN2(189) = "00" And
strhexbuffN2(190) = "FF" And
strhexbuffN2(191) = "FF" And
strhexbuffN2(192) = "FF" And
strhexbuffN2(193) = "00" And
strhexbuffN2(194) = "00" And
strhexbuffN2(195) = "19" And
strhexbuffN2(196) = "FF" And
strhexbuffN2(197) = "FF" And
strhexbuffN2(198) = "FF" And
strhexbuffN2(199) = "FF" And
strhexbuffN2(200) = "01" And
strhexbuffN2(201) = "00" And
strhexbuffN2(202) = "01" And
strhexbuffN2(203) = "00" And
strhexbuffN2(204) = "FF" And
strhexbuffN2(205) = "FF" And
strhexbuffN2(206) = "18" And
strhexbuffN2(207) = "03" And
strhexbuffN2(208) = "E3" And
strhexbuffN2(209) = "00" And
strhexbuffN2(210) = "0A" And
strhexbuffN2(211) = "00" And
strhexbuffN2(212) = "00" And
strhexbuffN2(213) = "FF" And
strhexbuffN2(214) = "E3" And
strhexbuffN2(215) = "00" And
strhexbuffN2(216) = "BE" And
strhexbuffN2(217) = "02" And
strhexbuffN2(218) = "E3" And
strhexbuffN2(219) = "00" And
strhexbuffN2(220) = "00" And
strhexbuffN2(221) = "00" And
strhexbuffN2(222) = "4C" And
strhexbuffN2(223) = "01" And
strhexbuffN2(224) = "00" And
strhexbuffN2(225) = "2A" And
strhexbuffN2(226) = "8D" And
strhexbuffN2(227) = "01" And
strhexbuffN2(228) = "00" And
strhexbuffN2(229) = "55" And
strhexbuffN2(230) = "D2" And
strhexbuffN2(231) = "01" And
strhexbuffN2(232) = "00" And
strhexbuffN2(233) = "7F" And
strhexbuffN2(234) = "19" And
strhexbuffN2(235) = "02" And
strhexbuffN2(236) = "00" And
strhexbuffN2(237) = "A9" And
strhexbuffN2(238) = "66" And
strhexbuffN2(239) = "02" And
strhexbuffN2(240) = "00" And
strhexbuffN2(241) = "D3" And
strhexbuffN2(242) = "BE" And
strhexbuffN2(243) = "02" And
strhexbuffN2(244) = "00" And
strhexbuffN2(245) = "FE" And
strhexbuffN2(246) = "3F" And
strhexbuffN2(247) = "FF" And
strhexbuffN2(248) = "F2" And
strhexbuffN2(249) = "F2" And
strhexbuffN2(250) = "F2" And
strhexbuffN2(251) = "F2" And
strhexbuffN2(252) = "3F" And
strhexbuffN2(253) = "3F" And
strhexbuffN2(254) = "3F" And
strhexbuffN2(255) = "3F" And
strhexbuffN2(256) = "3F" And
strhexbuffN2(257) = "3F" And
strhexbuffN2(258) = "3F" And
strhexbuffN2(259) = "3F" And
strhexbuffN2(260) = "3F" And
strhexbuffN2(261) = "3F" And
strhexbuffN2(262) = "3F" And
strhexbuffN2(263) = "3F" And
strhexbuffN2(264) = "3F" And
strhexbuffN2(265) = "3F" And
strhexbuffN2(266) = "3F" And
strhexbuffN2(267) = "3F" And
strhexbuffN2(268) = "3F" And
strhexbuffN2(269) = "3F" And
strhexbuffN2(270) = "3F" And
strhexbuffN2(271) = "3F" And
strhexbuffN2(272) = "3F" And
strhexbuffN2(273) = "3F" And
strhexbuffN2(274) = "3F" And
strhexbuffN2(275) = "3F" And
strhexbuffN2(276) = "3F" And
strhexbuffN2(277) = "3F" And
strhexbuffN2(278) = "3F" And
strhexbuffN2(279) = "3F" And
strhexbuffN2(280) = "3F" And
strhexbuffN2(281) = "3F" And
strhexbuffN2(282) = "3F" And
strhexbuffN2(283) = "3F" And
strhexbuffN2(284) = "3F" And
strhexbuffN2(285) = "3F" And
strhexbuffN2(286) = "3F" And
strhexbuffN2(287) = "3F" And
strhexbuffN2(288) = "3F" And
strhexbuffN2(289) = "3F" And
strhexbuffN2(290) = "3F" And
strhexbuffN2(291) = "3F" And
strhexbuffN2(292) = "3F" And
strhexbuffN2(293) = "3F" And
strhexbuffN2(294) = "3F" And
strhexbuffN2(295) = "3F" And
strhexbuffN2(296) = "3F" And
strhexbuffN2(297) = "3F" And
strhexbuffN2(298) = "3F" And
strhexbuffN2(299) = "3F" And
strhexbuffN2(300) = "3F" And
strhexbuffN2(301) = "3F" And
strhexbuffN2(302) = "3F" And
strhexbuffN2(303) = "3F" And
strhexbuffN2(304) = "3F" And
strhexbuffN2(305) = "3F" And
strhexbuffN2(306) = "3F" And
strhexbuffN2(307) = "3F" And
strhexbuffN2(308) = "3F" And
strhexbuffN2(309) = "3F" And
strhexbuffN2(310) = "E8" And
strhexbuffN2(311) = "03" And
strhexbuffN2(312) = "00" And
strhexbuffN2(313) = "00" And
strhexbuffN2(314) = "00" And
strhexbuffN2(315) = "00" And
strhexbuffN2(316) = "00" And
strhexbuffN2(317) = "00" And
strhexbuffN2(318) = "00" And
strhexbuffN2(319) = "00" And
strhexbuffN2(320) = "E8" And
strhexbuffN2(321) = "03" And
strhexbuffN2(322) = "00" And
strhexbuffN2(323) = "00" And
strhexbuffN2(324) = "70" And
strhexbuffN2(325) = "17" And
strhexbuffN2(326) = "00" And
strhexbuffN2(327) = "00" And
strhexbuffN2(328) = "03" And
strhexbuffN2(329) = "00" And
strhexbuffN2(330) = "00" And
strhexbuffN2(331) = "00" And
strhexbuffN2(332) = "00" And
strhexbuffN2(333) = "00" And
strhexbuffN2(334) = "00" And
strhexbuffN2(335) = "00" And
strhexbuffN2(336) = "00" And
strhexbuffN2(337) = "00" And
strhexbuffN2(338) = "00" And
strhexbuffN2(339) = "00" And
strhexbuffN2(340) = "00" And
strhexbuffN2(341) = "00" And
strhexbuffN2(342) = "00" And
strhexbuffN2(343) = "00" And
strhexbuffN2(344) = "00" And
strhexbuffN2(345) = "00" And
strhexbuffN2(346) = "00" And
strhexbuffN2(347) = "00" And
strhexbuffN2(348) = "00" And
strhexbuffN2(349) = "00" And
strhexbuffN2(350) = "00" And
strhexbuffN2(351) = "00" And
strhexbuffN2(352) = "00" And
strhexbuffN2(353) = "00" And
strhexbuffN2(354) = "00" And
strhexbuffN2(355) = "00" And
strhexbuffN2(356) = "00" And
strhexbuffN2(357) = "00" And
strhexbuffN2(358) = "00" And
strhexbuffN2(359) = "00" And
strhexbuffN2(360) = "00" And
strhexbuffN2(361) = "00" And
strhexbuffN2(362) = "00" And
strhexbuffN2(363) = "00" And
strhexbuffN2(364) = "00" And
strhexbuffN2(365) = "00" And
strhexbuffN2(366) = "00" And
strhexbuffN2(367) = "00" And
strhexbuffN2(368) = "00" And
strhexbuffN2(369) = "00" And
strhexbuffN2(370) = "00" And
strhexbuffN2(371) = "00" And
strhexbuffN2(372) = "00" And
strhexbuffN2(373) = "00" And
strhexbuffN2(374) = "00" And
strhexbuffN2(375) = "1D" And
strhexbuffN2(376) = "06" And
strhexbuffN2(377) = "01" And
strhexbuffN2(378) = "03" And
strhexbuffN2(379) = "14" And
strhexbuffN2(380) = "14" And
strhexbuffN2(381) = "FF" And
strhexbuffN2(382) = "02" And
strhexbuffN2(383) = "00" And
strhexbuffN2(384) = "04" And
strhexbuffN2(385) = "00" And
strhexbuffN2(386) = "00" And
strhexbuffN2(387) = "00" And
strhexbuffN2(388) = "00" And
strhexbuffN2(389) = "00" And
strhexbuffN2(390) = "01" And
strhexbuffN2(391) = "FF" And
strhexbuffN2(392) = "2C" And
strhexbuffN2(393) = "01" And
strhexbuffN2(394) = "00" And
strhexbuffN2(395) = "00" And
strhexbuffN2(396) = "00" And
strhexbuffN2(397) = "FF" And
strhexbuffN2(398) = "FF" And
strhexbuffN2(399) = "FF" And
strhexbuffN2(400) = "FF" And
strhexbuffN2(401) = "FF" And
strhexbuffN2(402) = "FF" And
strhexbuffN2(403) = "FF" And
strhexbuffN2(404) = "FF" And
strhexbuffN2(405) = "FF" And
strhexbuffN2(406) = "FF" And
strhexbuffN2(407) = "FF" And
strhexbuffN2(408) = "FF" And
strhexbuffN2(409) = "FF" And
strhexbuffN2(410) = "FF" And
strhexbuffN2(411) = "FF" And
strhexbuffN2(412) = "FF" And
strhexbuffN2(413) = "FF" And
strhexbuffN2(414) = "FF" And
strhexbuffN2(415) = "FF" And
strhexbuffN2(416) = "FF" And
strhexbuffN2(417) = "FF" And
strhexbuffN2(418) = "FF" And
strhexbuffN2(419) = "FF" And
strhexbuffN2(420) = "FF" And
strhexbuffN2(421) = "00" And
strhexbuffN2(422) = "FF" And
strhexbuffN2(423) = "FF" And
strhexbuffN2(424) = "FF" And
strhexbuffN2(425) = "FF" And
strhexbuffN2(426) = "FF" And
strhexbuffN2(427) = "FF" And
strhexbuffN2(428) = "FF" And
strhexbuffN2(429) = "FF" And
strhexbuffN2(430) = "FF" And
strhexbuffN2(431) = "00" And
strhexbuffN2(432) = "06" And
strhexbuffN2(433) = "00" And
strhexbuffN2(434) = "FE" And
strhexbuffN2(435) = "00" And
strhexbuffN2(436) = "06" And
strhexbuffN2(437) = "00" And
strhexbuffN2(438) = "FE" And
strhexbuffN2(439) = "00" And
strhexbuffN2(440) = "06" And
strhexbuffN2(441) = "00" And
strhexbuffN2(442) = "FE" And
strhexbuffN2(443) = "00" And
strhexbuffN2(444) = "06" And
strhexbuffN2(445) = "00" And
strhexbuffN2(446) = "FE" And
strhexbuffN2(447) = "00" And
strhexbuffN2(448) = "06" And
strhexbuffN2(449) = "00" And
strhexbuffN2(450) = "FE" And
strhexbuffN2(451) = "00" And
strhexbuffN2(452) = "06" And
strhexbuffN2(453) = "00" And
strhexbuffN2(454) = "FE" And
strhexbuffN2(455) = "00" And
strhexbuffN2(456) = "06" And
strhexbuffN2(457) = "00" And
strhexbuffN2(458) = "FE" And
strhexbuffN2(459) = "00" And
strhexbuffN2(460) = "06" And
strhexbuffN2(461) = "00" And
strhexbuffN2(462) = "FE" And
strhexbuffN2(463) = "00" And
strhexbuffN2(464) = "06" And
strhexbuffN2(465) = "00" And
strhexbuffN2(466) = "FE" And
strhexbuffN2(467) = "00" And
strhexbuffN2(468) = "06" And
strhexbuffN2(469) = "00" And
strhexbuffN2(470) = "FE" And
strhexbuffN2(471) = "00" And
strhexbuffN2(472) = "06" And
strhexbuffN2(473) = "00" And
strhexbuffN2(474) = "FE" And
strhexbuffN2(475) = "00" And
strhexbuffN2(476) = "06" And
strhexbuffN2(477) = "00" And
strhexbuffN2(478) = "FE" And
strhexbuffN2(479) = "00" And
strhexbuffN2(480) = "06" And
strhexbuffN2(481) = "00" And
strhexbuffN2(482) = "FE" And
strhexbuffN2(483) = "00" And
strhexbuffN2(484) = "06" And
strhexbuffN2(485) = "00" And
strhexbuffN2(486) = "FE" And
strhexbuffN2(487) = "00" And
strhexbuffN2(488) = "06" And
strhexbuffN2(489) = "00" And
strhexbuffN2(490) = "FE" And
strhexbuffN2(491) = "00" And
strhexbuffN2(492) = "06" And
strhexbuffN2(493) = "00" And
strhexbuffN2(494) = "FE" And
strhexbuffN2(495) = "00" And
strhexbuffN2(496) = "06" And
strhexbuffN2(497) = "00" And
strhexbuffN2(498) = "FE" And
strhexbuffN2(499) = "00" And
strhexbuffN2(500) = "05" And
strhexbuffN2(501) = "00" And
strhexbuffN2(502) = "00" And
strhexbuffN2(503) = "00" And
strhexbuffN2(504) = "05" And
strhexbuffN2(505) = "00" And
strhexbuffN2(506) = "00" And
strhexbuffN2(507) = "00" And
strhexbuffN2(508) = "F4" And
strhexbuffN2(509) = "F4" And
strhexbuffN2(510) = "F4" And
strhexbuffN2(511) = "F4" Then
            ValidateNVMN2 = True
        Else
            ValidateNVMN2 = False
        End If

    End Sub

    Public Sub CheckNVMDataN3()
        If strhexbuffN3(0) = "FF" And
strhexbuffN3(1) = "00" And
strhexbuffN3(2) = "3F" And
strhexbuffN3(3) = "3F" And
strhexbuffN3(4) = "01" And
strhexbuffN3(5) = "06" And
strhexbuffN3(6) = "01" And
strhexbuffN3(7) = "00" And
strhexbuffN3(8) = "F6" And
strhexbuffN3(9) = "00" And
strhexbuffN3(10) = "3F" And
strhexbuffN3(11) = "3F" And
strhexbuffN3(12) = "27" And
strhexbuffN3(13) = "8D" And
strhexbuffN3(14) = "0A" And
strhexbuffN3(15) = "09" And
strhexbuffN3(16) = "12" And
strhexbuffN3(17) = "FF" And
strhexbuffN3(18) = "00" And
strhexbuffN3(19) = "00" And
strhexbuffN3(20) = "80" And
strhexbuffN3(21) = "3E" And
strhexbuffN3(22) = "00" And
strhexbuffN3(23) = "00" And
strhexbuffN3(24) = "00" And
strhexbuffN3(25) = "00" And
strhexbuffN3(26) = "D0" And
strhexbuffN3(27) = "07" And
strhexbuffN3(28) = "FC" And
strhexbuffN3(29) = "08" And
strhexbuffN3(30) = "A0" And
strhexbuffN3(31) = "0F" And
strhexbuffN3(32) = "94" And
strhexbuffN3(33) = "11" And
strhexbuffN3(34) = "70" And
strhexbuffN3(35) = "17" And
strhexbuffN3(36) = "2C" And
strhexbuffN3(37) = "1A" And
strhexbuffN3(38) = "40" And
strhexbuffN3(39) = "1F" And
strhexbuffN3(40) = "C4" And
strhexbuffN3(41) = "22" And
strhexbuffN3(42) = "10" And
strhexbuffN3(43) = "27" And
strhexbuffN3(44) = "C0" And
strhexbuffN3(45) = "2B" And
strhexbuffN3(46) = "E0" And
strhexbuffN3(47) = "2E" And
strhexbuffN3(48) = "58" And
strhexbuffN3(49) = "34" And
strhexbuffN3(50) = "B0" And
strhexbuffN3(51) = "36" And
strhexbuffN3(52) = "BE" And
strhexbuffN3(53) = "3C" And
strhexbuffN3(54) = "80" And
strhexbuffN3(55) = "3E" And
strhexbuffN3(56) = "8E" And
strhexbuffN3(57) = "44" And
strhexbuffN3(58) = "FF" And
strhexbuffN3(59) = "FF" And
strhexbuffN3(60) = "FF" And
strhexbuffN3(61) = "FF" And
strhexbuffN3(62) = "FF" And
strhexbuffN3(63) = "FF" And
strhexbuffN3(64) = "FF" And
strhexbuffN3(65) = "FF" And
strhexbuffN3(66) = "00" And
strhexbuffN3(67) = "00" And
strhexbuffN3(68) = "80" And
strhexbuffN3(69) = "3E" And
strhexbuffN3(70) = "00" And
strhexbuffN3(71) = "00" And
strhexbuffN3(72) = "00" And
strhexbuffN3(73) = "00" And
strhexbuffN3(74) = "D0" And
strhexbuffN3(75) = "07" And
strhexbuffN3(76) = "FC" And
strhexbuffN3(77) = "08" And
strhexbuffN3(78) = "A0" And
strhexbuffN3(79) = "0F" And
strhexbuffN3(80) = "94" And
strhexbuffN3(81) = "11" And
strhexbuffN3(82) = "70" And
strhexbuffN3(83) = "17" And
strhexbuffN3(84) = "2C" And
strhexbuffN3(85) = "1A" And
strhexbuffN3(86) = "40" And
strhexbuffN3(87) = "1F" And
strhexbuffN3(88) = "C4" And
strhexbuffN3(89) = "22" And
strhexbuffN3(90) = "10" And
strhexbuffN3(91) = "27" And
strhexbuffN3(92) = "C0" And
strhexbuffN3(93) = "2B" And
strhexbuffN3(94) = "E0" And
strhexbuffN3(95) = "2E" And
strhexbuffN3(96) = "58" And
strhexbuffN3(97) = "34" And
strhexbuffN3(98) = "B0" And
strhexbuffN3(99) = "36" And
strhexbuffN3(100) = "BE" And
strhexbuffN3(101) = "3C" And
strhexbuffN3(102) = "80" And
strhexbuffN3(103) = "3E" And
strhexbuffN3(104) = "8E" And
strhexbuffN3(105) = "44" And
strhexbuffN3(106) = "FF" And
strhexbuffN3(107) = "FF" And
strhexbuffN3(108) = "FF" And
strhexbuffN3(109) = "FF" And
strhexbuffN3(110) = "FF" And
strhexbuffN3(111) = "FF" And
strhexbuffN3(112) = "FF" And
strhexbuffN3(113) = "FF" And
strhexbuffN3(114) = "00" And
strhexbuffN3(115) = "00" And
strhexbuffN3(116) = "80" And
strhexbuffN3(117) = "3E" And
strhexbuffN3(118) = "00" And
strhexbuffN3(119) = "00" And
strhexbuffN3(120) = "00" And
strhexbuffN3(121) = "00" And
strhexbuffN3(122) = "D0" And
strhexbuffN3(123) = "07" And
strhexbuffN3(124) = "66" And
strhexbuffN3(125) = "08" And
strhexbuffN3(126) = "A0" And
strhexbuffN3(127) = "0F" And
strhexbuffN3(128) = "CC" And
strhexbuffN3(129) = "10" And
strhexbuffN3(130) = "70" And
strhexbuffN3(131) = "17" And
strhexbuffN3(132) = "64" And
strhexbuffN3(133) = "19" And
strhexbuffN3(134) = "40" And
strhexbuffN3(135) = "1F" And
strhexbuffN3(136) = "FC" And
strhexbuffN3(137) = "21" And
strhexbuffN3(138) = "10" And
strhexbuffN3(139) = "27" And
strhexbuffN3(140) = "F8" And
strhexbuffN3(141) = "2A" And
strhexbuffN3(142) = "E0" And
strhexbuffN3(143) = "2E" And
strhexbuffN3(144) = "90" And
strhexbuffN3(145) = "33" And
strhexbuffN3(146) = "B0" And
strhexbuffN3(147) = "36" And
strhexbuffN3(148) = "8C" And
strhexbuffN3(149) = "3C" And
strhexbuffN3(150) = "80" And
strhexbuffN3(151) = "3E" And
strhexbuffN3(152) = "5C" And
strhexbuffN3(153) = "44" And
strhexbuffN3(154) = "FF" And
strhexbuffN3(155) = "FF" And
strhexbuffN3(156) = "FF" And
strhexbuffN3(157) = "FF" And
strhexbuffN3(158) = "FF" And
strhexbuffN3(159) = "FF" And
strhexbuffN3(160) = "FF" And
strhexbuffN3(161) = "FF" And
strhexbuffN3(162) = "4A" And
strhexbuffN3(163) = "6D" And
strhexbuffN3(164) = "4A" And
strhexbuffN3(165) = "6D" And
strhexbuffN3(166) = "02" And
strhexbuffN3(167) = "05" And
strhexbuffN3(168) = "32" And
strhexbuffN3(169) = "00" And
strhexbuffN3(170) = "32" And
strhexbuffN3(171) = "00" And
strhexbuffN3(172) = "04" And
strhexbuffN3(173) = "00" And
strhexbuffN3(174) = "B7" And
strhexbuffN3(175) = "3F" And
strhexbuffN3(176) = "48" And
strhexbuffN3(177) = "00" And
strhexbuffN3(178) = "00" And
strhexbuffN3(179) = "00" And
strhexbuffN3(180) = "FF" And
strhexbuffN3(181) = "FF" And
strhexbuffN3(182) = "FF" And
strhexbuffN3(183) = "00" And
strhexbuffN3(184) = "00" And
strhexbuffN3(185) = "00" And
strhexbuffN3(186) = "A4" And
strhexbuffN3(187) = "70" And
strhexbuffN3(188) = "3D" And
strhexbuffN3(189) = "00" And
strhexbuffN3(190) = "FF" And
strhexbuffN3(191) = "FF" And
strhexbuffN3(192) = "FF" And
strhexbuffN3(193) = "00" And
strhexbuffN3(194) = "00" And
strhexbuffN3(195) = "19" And
strhexbuffN3(196) = "FF" And
strhexbuffN3(197) = "FF" And
strhexbuffN3(198) = "FF" And
strhexbuffN3(199) = "FF" And
strhexbuffN3(200) = "01" And
strhexbuffN3(201) = "00" And
strhexbuffN3(202) = "01" And
strhexbuffN3(203) = "00" And
strhexbuffN3(204) = "FF" And
strhexbuffN3(205) = "FF" And
strhexbuffN3(206) = "18" And
strhexbuffN3(207) = "03" And
strhexbuffN3(208) = "E3" And
strhexbuffN3(209) = "00" And
strhexbuffN3(210) = "0A" And
strhexbuffN3(211) = "00" And
strhexbuffN3(212) = "00" And
strhexbuffN3(213) = "FF" And
strhexbuffN3(214) = "E3" And
strhexbuffN3(215) = "00" And
strhexbuffN3(216) = "BE" And
strhexbuffN3(217) = "02" And
strhexbuffN3(218) = "E3" And
strhexbuffN3(219) = "00" And
strhexbuffN3(220) = "00" And
strhexbuffN3(221) = "00" And
strhexbuffN3(222) = "4C" And
strhexbuffN3(223) = "01" And
strhexbuffN3(224) = "00" And
strhexbuffN3(225) = "2A" And
strhexbuffN3(226) = "8D" And
strhexbuffN3(227) = "01" And
strhexbuffN3(228) = "00" And
strhexbuffN3(229) = "55" And
strhexbuffN3(230) = "D2" And
strhexbuffN3(231) = "01" And
strhexbuffN3(232) = "00" And
strhexbuffN3(233) = "7F" And
strhexbuffN3(234) = "19" And
strhexbuffN3(235) = "02" And
strhexbuffN3(236) = "00" And
strhexbuffN3(237) = "A9" And
strhexbuffN3(238) = "66" And
strhexbuffN3(239) = "02" And
strhexbuffN3(240) = "00" And
strhexbuffN3(241) = "D3" And
strhexbuffN3(242) = "BE" And
strhexbuffN3(243) = "02" And
strhexbuffN3(244) = "00" And
strhexbuffN3(245) = "FE" And
strhexbuffN3(246) = "3F" And
strhexbuffN3(247) = "FF" And
strhexbuffN3(248) = "F2" And
strhexbuffN3(249) = "F2" And
strhexbuffN3(250) = "F2" And
strhexbuffN3(251) = "F2" And
strhexbuffN3(252) = "3F" And
strhexbuffN3(253) = "3F" And
strhexbuffN3(254) = "3F" And
strhexbuffN3(255) = "3F" And
strhexbuffN3(256) = "3F" And
strhexbuffN3(257) = "3F" And
strhexbuffN3(258) = "3F" And
strhexbuffN3(259) = "3F" And
strhexbuffN3(260) = "3F" And
strhexbuffN3(261) = "3F" And
strhexbuffN3(262) = "3F" And
strhexbuffN3(263) = "3F" And
strhexbuffN3(264) = "3F" And
strhexbuffN3(265) = "3F" And
strhexbuffN3(266) = "3F" And
strhexbuffN3(267) = "3F" And
strhexbuffN3(268) = "3F" And
strhexbuffN3(269) = "3F" And
strhexbuffN3(270) = "3F" And
strhexbuffN3(271) = "3F" And
strhexbuffN3(272) = "3F" And
strhexbuffN3(273) = "3F" And
strhexbuffN3(274) = "3F" And
strhexbuffN3(275) = "3F" And
strhexbuffN3(276) = "3F" And
strhexbuffN3(277) = "3F" And
strhexbuffN3(278) = "3F" And
strhexbuffN3(279) = "3F" And
strhexbuffN3(280) = "3F" And
strhexbuffN3(281) = "3F" And
strhexbuffN3(282) = "3F" And
strhexbuffN3(283) = "3F" And
strhexbuffN3(284) = "3F" And
strhexbuffN3(285) = "3F" And
strhexbuffN3(286) = "3F" And
strhexbuffN3(287) = "3F" And
strhexbuffN3(288) = "3F" And
strhexbuffN3(289) = "3F" And
strhexbuffN3(290) = "3F" And
strhexbuffN3(291) = "3F" And
strhexbuffN3(292) = "3F" And
strhexbuffN3(293) = "3F" And
strhexbuffN3(294) = "3F" And
strhexbuffN3(295) = "3F" And
strhexbuffN3(296) = "3F" And
strhexbuffN3(297) = "3F" And
strhexbuffN3(298) = "3F" And
strhexbuffN3(299) = "3F" And
strhexbuffN3(300) = "3F" And
strhexbuffN3(301) = "3F" And
strhexbuffN3(302) = "3F" And
strhexbuffN3(303) = "3F" And
strhexbuffN3(304) = "3F" And
strhexbuffN3(305) = "3F" And
strhexbuffN3(306) = "3F" And
strhexbuffN3(307) = "3F" And
strhexbuffN3(308) = "3F" And
strhexbuffN3(309) = "3F" And
strhexbuffN3(310) = "E8" And
strhexbuffN3(311) = "03" And
strhexbuffN3(312) = "00" And
strhexbuffN3(313) = "00" And
strhexbuffN3(314) = "00" And
strhexbuffN3(315) = "00" And
strhexbuffN3(316) = "00" And
strhexbuffN3(317) = "00" And
strhexbuffN3(318) = "00" And
strhexbuffN3(319) = "00" And
strhexbuffN3(320) = "E8" And
strhexbuffN3(321) = "03" And
strhexbuffN3(322) = "00" And
strhexbuffN3(323) = "00" And
strhexbuffN3(324) = "70" And
strhexbuffN3(325) = "17" And
strhexbuffN3(326) = "00" And
strhexbuffN3(327) = "00" And
strhexbuffN3(328) = "03" And
strhexbuffN3(329) = "00" And
strhexbuffN3(330) = "00" And
strhexbuffN3(331) = "00" And
strhexbuffN3(332) = "00" And
strhexbuffN3(333) = "00" And
strhexbuffN3(334) = "00" And
strhexbuffN3(335) = "00" And
strhexbuffN3(336) = "00" And
strhexbuffN3(337) = "00" And
strhexbuffN3(338) = "00" And
strhexbuffN3(339) = "00" And
strhexbuffN3(340) = "00" And
strhexbuffN3(341) = "00" And
strhexbuffN3(342) = "00" And
strhexbuffN3(343) = "00" And
strhexbuffN3(344) = "00" And
strhexbuffN3(345) = "00" And
strhexbuffN3(346) = "00" And
strhexbuffN3(347) = "00" And
strhexbuffN3(348) = "00" And
strhexbuffN3(349) = "00" And
strhexbuffN3(350) = "00" And
strhexbuffN3(351) = "00" And
strhexbuffN3(352) = "00" And
strhexbuffN3(353) = "00" And
strhexbuffN3(354) = "00" And
strhexbuffN3(355) = "00" And
strhexbuffN3(356) = "00" And
strhexbuffN3(357) = "00" And
strhexbuffN3(358) = "00" And
strhexbuffN3(359) = "00" And
strhexbuffN3(360) = "00" And
strhexbuffN3(361) = "00" And
strhexbuffN3(362) = "00" And
strhexbuffN3(363) = "00" And
strhexbuffN3(364) = "00" And
strhexbuffN3(365) = "00" And
strhexbuffN3(366) = "00" And
strhexbuffN3(367) = "00" And
strhexbuffN3(368) = "00" And
strhexbuffN3(369) = "00" And
strhexbuffN3(370) = "00" And
strhexbuffN3(371) = "00" And
strhexbuffN3(372) = "00" And
strhexbuffN3(373) = "00" And
strhexbuffN3(374) = "00" And
strhexbuffN3(375) = "1D" And
strhexbuffN3(376) = "06" And
strhexbuffN3(377) = "01" And
strhexbuffN3(378) = "03" And
strhexbuffN3(379) = "14" And
strhexbuffN3(380) = "14" And
strhexbuffN3(381) = "FF" And
strhexbuffN3(382) = "02" And
strhexbuffN3(383) = "00" And
strhexbuffN3(384) = "04" And
strhexbuffN3(385) = "00" And
strhexbuffN3(386) = "00" And
strhexbuffN3(387) = "00" And
strhexbuffN3(388) = "00" And
strhexbuffN3(389) = "00" And
strhexbuffN3(390) = "01" And
strhexbuffN3(391) = "FF" And
strhexbuffN3(392) = "2C" And
strhexbuffN3(393) = "01" And
strhexbuffN3(394) = "00" And
strhexbuffN3(395) = "00" And
strhexbuffN3(396) = "00" And
strhexbuffN3(397) = "FF" And
strhexbuffN3(398) = "FF" And
strhexbuffN3(399) = "FF" And
strhexbuffN3(400) = "FF" And
strhexbuffN3(401) = "FF" And
strhexbuffN3(402) = "FF" And
strhexbuffN3(403) = "FF" And
strhexbuffN3(404) = "FF" And
strhexbuffN3(405) = "FF" And
strhexbuffN3(406) = "FF" And
strhexbuffN3(407) = "FF" And
strhexbuffN3(408) = "FF" And
strhexbuffN3(409) = "FF" And
strhexbuffN3(410) = "FF" And
strhexbuffN3(411) = "FF" And
strhexbuffN3(412) = "FF" And
strhexbuffN3(413) = "FF" And
strhexbuffN3(414) = "FF" And
strhexbuffN3(415) = "FF" And
strhexbuffN3(416) = "FF" And
strhexbuffN3(417) = "FF" And
strhexbuffN3(418) = "FF" And
strhexbuffN3(419) = "FF" And
strhexbuffN3(420) = "FF" And
strhexbuffN3(421) = "00" And
strhexbuffN3(422) = "FF" And
strhexbuffN3(423) = "FF" And
strhexbuffN3(424) = "FF" And
strhexbuffN3(425) = "FF" And
strhexbuffN3(426) = "FF" And
strhexbuffN3(427) = "FF" And
strhexbuffN3(428) = "FF" And
strhexbuffN3(429) = "FF" And
strhexbuffN3(430) = "FF" And
strhexbuffN3(431) = "00" And
strhexbuffN3(432) = "06" And
strhexbuffN3(433) = "00" And
strhexbuffN3(434) = "FE" And
strhexbuffN3(435) = "00" And
strhexbuffN3(436) = "06" And
strhexbuffN3(437) = "00" And
strhexbuffN3(438) = "FE" And
strhexbuffN3(439) = "00" And
strhexbuffN3(440) = "06" And
strhexbuffN3(441) = "00" And
strhexbuffN3(442) = "FE" And
strhexbuffN3(443) = "00" And
strhexbuffN3(444) = "06" And
strhexbuffN3(445) = "00" And
strhexbuffN3(446) = "FE" And
strhexbuffN3(447) = "00" And
strhexbuffN3(448) = "06" And
strhexbuffN3(449) = "00" And
strhexbuffN3(450) = "FE" And
strhexbuffN3(451) = "00" And
strhexbuffN3(452) = "06" And
strhexbuffN3(453) = "00" And
strhexbuffN3(454) = "FE" And
strhexbuffN3(455) = "00" And
strhexbuffN3(456) = "06" And
strhexbuffN3(457) = "00" And
strhexbuffN3(458) = "FE" And
strhexbuffN3(459) = "00" And
strhexbuffN3(460) = "06" And
strhexbuffN3(461) = "00" And
strhexbuffN3(462) = "FE" And
strhexbuffN3(463) = "00" And
strhexbuffN3(464) = "06" And
strhexbuffN3(465) = "00" And
strhexbuffN3(466) = "FE" And
strhexbuffN3(467) = "00" And
strhexbuffN3(468) = "06" And
strhexbuffN3(469) = "00" And
strhexbuffN3(470) = "FE" And
strhexbuffN3(471) = "00" And
strhexbuffN3(472) = "06" And
strhexbuffN3(473) = "00" And
strhexbuffN3(474) = "FE" And
strhexbuffN3(475) = "00" And
strhexbuffN3(476) = "06" And
strhexbuffN3(477) = "00" And
strhexbuffN3(478) = "FE" And
strhexbuffN3(479) = "00" And
strhexbuffN3(480) = "06" And
strhexbuffN3(481) = "00" And
strhexbuffN3(482) = "FE" And
strhexbuffN3(483) = "00" And
strhexbuffN3(484) = "06" And
strhexbuffN3(485) = "00" And
strhexbuffN3(486) = "FE" And
strhexbuffN3(487) = "00" And
strhexbuffN3(488) = "06" And
strhexbuffN3(489) = "00" And
strhexbuffN3(490) = "FE" And
strhexbuffN3(491) = "00" And
strhexbuffN3(492) = "06" And
strhexbuffN3(493) = "00" And
strhexbuffN3(494) = "FE" And
strhexbuffN3(495) = "00" And
strhexbuffN3(496) = "06" And
strhexbuffN3(497) = "00" And
strhexbuffN3(498) = "FE" And
strhexbuffN3(499) = "00" And
strhexbuffN3(500) = "05" And
strhexbuffN3(501) = "00" And
strhexbuffN3(502) = "00" And
strhexbuffN3(503) = "00" And
strhexbuffN3(504) = "05" And
strhexbuffN3(505) = "00" And
strhexbuffN3(506) = "00" And
strhexbuffN3(507) = "00" And
strhexbuffN3(508) = "F4" And
strhexbuffN3(509) = "F4" And
strhexbuffN3(510) = "F4" And
strhexbuffN3(511) = "F4" Then
            ValidateNVMN3 = True
        Else
            ValidateNVMN3 = False
        End If

    End Sub

    Public Sub CheckNVMDataN4()
        If strhexbuffN4(0) = "FF" And
strhexbuffN4(1) = "00" And
strhexbuffN4(2) = "3F" And
strhexbuffN4(3) = "3F" And
strhexbuffN4(4) = "01" And
strhexbuffN4(5) = "06" And
strhexbuffN4(6) = "01" And
strhexbuffN4(7) = "00" And
strhexbuffN4(8) = "F6" And
strhexbuffN4(9) = "00" And
strhexbuffN4(10) = "3F" And
strhexbuffN4(11) = "3F" And
strhexbuffN4(12) = "27" And
strhexbuffN4(13) = "8D" And
strhexbuffN4(14) = "0A" And
strhexbuffN4(15) = "09" And
strhexbuffN4(16) = "12" And
strhexbuffN4(17) = "FF" And
strhexbuffN4(18) = "00" And
strhexbuffN4(19) = "00" And
strhexbuffN4(20) = "80" And
strhexbuffN4(21) = "3E" And
strhexbuffN4(22) = "00" And
strhexbuffN4(23) = "00" And
strhexbuffN4(24) = "00" And
strhexbuffN4(25) = "00" And
strhexbuffN4(26) = "D0" And
strhexbuffN4(27) = "07" And
strhexbuffN4(28) = "FC" And
strhexbuffN4(29) = "08" And
strhexbuffN4(30) = "A0" And
strhexbuffN4(31) = "0F" And
strhexbuffN4(32) = "94" And
strhexbuffN4(33) = "11" And
strhexbuffN4(34) = "70" And
strhexbuffN4(35) = "17" And
strhexbuffN4(36) = "2C" And
strhexbuffN4(37) = "1A" And
strhexbuffN4(38) = "40" And
strhexbuffN4(39) = "1F" And
strhexbuffN4(40) = "C4" And
strhexbuffN4(41) = "22" And
strhexbuffN4(42) = "10" And
strhexbuffN4(43) = "27" And
strhexbuffN4(44) = "C0" And
strhexbuffN4(45) = "2B" And
strhexbuffN4(46) = "E0" And
strhexbuffN4(47) = "2E" And
strhexbuffN4(48) = "58" And
strhexbuffN4(49) = "34" And
strhexbuffN4(50) = "B0" And
strhexbuffN4(51) = "36" And
strhexbuffN4(52) = "BE" And
strhexbuffN4(53) = "3C" And
strhexbuffN4(54) = "80" And
strhexbuffN4(55) = "3E" And
strhexbuffN4(56) = "8E" And
strhexbuffN4(57) = "44" And
strhexbuffN4(58) = "FF" And
strhexbuffN4(59) = "FF" And
strhexbuffN4(60) = "FF" And
strhexbuffN4(61) = "FF" And
strhexbuffN4(62) = "FF" And
strhexbuffN4(63) = "FF" And
strhexbuffN4(64) = "FF" And
strhexbuffN4(65) = "FF" And
strhexbuffN4(66) = "00" And
strhexbuffN4(67) = "00" And
strhexbuffN4(68) = "80" And
strhexbuffN4(69) = "3E" And
strhexbuffN4(70) = "00" And
strhexbuffN4(71) = "00" And
strhexbuffN4(72) = "00" And
strhexbuffN4(73) = "00" And
strhexbuffN4(74) = "D0" And
strhexbuffN4(75) = "07" And
strhexbuffN4(76) = "FC" And
strhexbuffN4(77) = "08" And
strhexbuffN4(78) = "A0" And
strhexbuffN4(79) = "0F" And
strhexbuffN4(80) = "94" And
strhexbuffN4(81) = "11" And
strhexbuffN4(82) = "70" And
strhexbuffN4(83) = "17" And
strhexbuffN4(84) = "2C" And
strhexbuffN4(85) = "1A" And
strhexbuffN4(86) = "40" And
strhexbuffN4(87) = "1F" And
strhexbuffN4(88) = "C4" And
strhexbuffN4(89) = "22" And
strhexbuffN4(90) = "10" And
strhexbuffN4(91) = "27" And
strhexbuffN4(92) = "C0" And
strhexbuffN4(93) = "2B" And
strhexbuffN4(94) = "E0" And
strhexbuffN4(95) = "2E" And
strhexbuffN4(96) = "58" And
strhexbuffN4(97) = "34" And
strhexbuffN4(98) = "B0" And
strhexbuffN4(99) = "36" And
strhexbuffN4(100) = "BE" And
strhexbuffN4(101) = "3C" And
strhexbuffN4(102) = "80" And
strhexbuffN4(103) = "3E" And
strhexbuffN4(104) = "8E" And
strhexbuffN4(105) = "44" And
strhexbuffN4(106) = "FF" And
strhexbuffN4(107) = "FF" And
strhexbuffN4(108) = "FF" And
strhexbuffN4(109) = "FF" And
strhexbuffN4(110) = "FF" And
strhexbuffN4(111) = "FF" And
strhexbuffN4(112) = "FF" And
strhexbuffN4(113) = "FF" And
strhexbuffN4(114) = "00" And
strhexbuffN4(115) = "00" And
strhexbuffN4(116) = "80" And
strhexbuffN4(117) = "3E" And
strhexbuffN4(118) = "00" And
strhexbuffN4(119) = "00" And
strhexbuffN4(120) = "00" And
strhexbuffN4(121) = "00" And
strhexbuffN4(122) = "D0" And
strhexbuffN4(123) = "07" And
strhexbuffN4(124) = "66" And
strhexbuffN4(125) = "08" And
strhexbuffN4(126) = "A0" And
strhexbuffN4(127) = "0F" And
strhexbuffN4(128) = "CC" And
strhexbuffN4(129) = "10" And
strhexbuffN4(130) = "70" And
strhexbuffN4(131) = "17" And
strhexbuffN4(132) = "64" And
strhexbuffN4(133) = "19" And
strhexbuffN4(134) = "40" And
strhexbuffN4(135) = "1F" And
strhexbuffN4(136) = "FC" And
strhexbuffN4(137) = "21" And
strhexbuffN4(138) = "10" And
strhexbuffN4(139) = "27" And
strhexbuffN4(140) = "F8" And
strhexbuffN4(141) = "2A" And
strhexbuffN4(142) = "E0" And
strhexbuffN4(143) = "2E" And
strhexbuffN4(144) = "90" And
strhexbuffN4(145) = "33" And
strhexbuffN4(146) = "B0" And
strhexbuffN4(147) = "36" And
strhexbuffN4(148) = "8C" And
strhexbuffN4(149) = "3C" And
strhexbuffN4(150) = "80" And
strhexbuffN4(151) = "3E" And
strhexbuffN4(152) = "5C" And
strhexbuffN4(153) = "44" And
strhexbuffN4(154) = "FF" And
strhexbuffN4(155) = "FF" And
strhexbuffN4(156) = "FF" And
strhexbuffN4(157) = "FF" And
strhexbuffN4(158) = "FF" And
strhexbuffN4(159) = "FF" And
strhexbuffN4(160) = "FF" And
strhexbuffN4(161) = "FF" And
strhexbuffN4(162) = "4A" And
strhexbuffN4(163) = "6D" And
strhexbuffN4(164) = "4A" And
strhexbuffN4(165) = "6D" And
strhexbuffN4(166) = "02" And
strhexbuffN4(167) = "05" And
strhexbuffN4(168) = "32" And
strhexbuffN4(169) = "00" And
strhexbuffN4(170) = "32" And
strhexbuffN4(171) = "00" And
strhexbuffN4(172) = "04" And
strhexbuffN4(173) = "00" And
strhexbuffN4(174) = "B7" And
strhexbuffN4(175) = "3F" And
strhexbuffN4(176) = "48" And
strhexbuffN4(177) = "00" And
strhexbuffN4(178) = "00" And
strhexbuffN4(179) = "00" And
strhexbuffN4(180) = "FF" And
strhexbuffN4(181) = "FF" And
strhexbuffN4(182) = "FF" And
strhexbuffN4(183) = "00" And
strhexbuffN4(184) = "00" And
strhexbuffN4(185) = "00" And
strhexbuffN4(186) = "A4" And
strhexbuffN4(187) = "70" And
strhexbuffN4(188) = "3D" And
strhexbuffN4(189) = "00" And
strhexbuffN4(190) = "FF" And
strhexbuffN4(191) = "FF" And
strhexbuffN4(192) = "FF" And
strhexbuffN4(193) = "00" And
strhexbuffN4(194) = "00" And
strhexbuffN4(195) = "19" And
strhexbuffN4(196) = "FF" And
strhexbuffN4(197) = "FF" And
strhexbuffN4(198) = "FF" And
strhexbuffN4(199) = "FF" And
strhexbuffN4(200) = "01" And
strhexbuffN4(201) = "00" And
strhexbuffN4(202) = "01" And
strhexbuffN4(203) = "00" And
strhexbuffN4(204) = "FF" And
strhexbuffN4(205) = "FF" And
strhexbuffN4(206) = "18" And
strhexbuffN4(207) = "03" And
strhexbuffN4(208) = "E3" And
strhexbuffN4(209) = "00" And
strhexbuffN4(210) = "0A" And
strhexbuffN4(211) = "00" And
strhexbuffN4(212) = "00" And
strhexbuffN4(213) = "FF" And
strhexbuffN4(214) = "E3" And
strhexbuffN4(215) = "00" And
strhexbuffN4(216) = "BE" And
strhexbuffN4(217) = "02" And
strhexbuffN4(218) = "E3" And
strhexbuffN4(219) = "00" And
strhexbuffN4(220) = "00" And
strhexbuffN4(221) = "00" And
strhexbuffN4(222) = "4C" And
strhexbuffN4(223) = "01" And
strhexbuffN4(224) = "00" And
strhexbuffN4(225) = "2A" And
strhexbuffN4(226) = "8D" And
strhexbuffN4(227) = "01" And
strhexbuffN4(228) = "00" And
strhexbuffN4(229) = "55" And
strhexbuffN4(230) = "D2" And
strhexbuffN4(231) = "01" And
strhexbuffN4(232) = "00" And
strhexbuffN4(233) = "7F" And
strhexbuffN4(234) = "19" And
strhexbuffN4(235) = "02" And
strhexbuffN4(236) = "00" And
strhexbuffN4(237) = "A9" And
strhexbuffN4(238) = "66" And
strhexbuffN4(239) = "02" And
strhexbuffN4(240) = "00" And
strhexbuffN4(241) = "D3" And
strhexbuffN4(242) = "BE" And
strhexbuffN4(243) = "02" And
strhexbuffN4(244) = "00" And
strhexbuffN4(245) = "FE" And
strhexbuffN4(246) = "3F" And
strhexbuffN4(247) = "FF" And
strhexbuffN4(248) = "F2" And
strhexbuffN4(249) = "F2" And
strhexbuffN4(250) = "F2" And
strhexbuffN4(251) = "F2" And
strhexbuffN4(252) = "3F" And
strhexbuffN4(253) = "3F" And
strhexbuffN4(254) = "3F" And
strhexbuffN4(255) = "3F" And
strhexbuffN4(256) = "3F" And
strhexbuffN4(257) = "3F" And
strhexbuffN4(258) = "3F" And
strhexbuffN4(259) = "3F" And
strhexbuffN4(260) = "3F" And
strhexbuffN4(261) = "3F" And
strhexbuffN4(262) = "3F" And
strhexbuffN4(263) = "3F" And
strhexbuffN4(264) = "3F" And
strhexbuffN4(265) = "3F" And
strhexbuffN4(266) = "3F" And
strhexbuffN4(267) = "3F" And
strhexbuffN4(268) = "3F" And
strhexbuffN4(269) = "3F" And
strhexbuffN4(270) = "3F" And
strhexbuffN4(271) = "3F" And
strhexbuffN4(272) = "3F" And
strhexbuffN4(273) = "3F" And
strhexbuffN4(274) = "3F" And
strhexbuffN4(275) = "3F" And
strhexbuffN4(276) = "3F" And
strhexbuffN4(277) = "3F" And
strhexbuffN4(278) = "3F" And
strhexbuffN4(279) = "3F" And
strhexbuffN4(280) = "3F" And
strhexbuffN4(281) = "3F" And
strhexbuffN4(282) = "3F" And
strhexbuffN4(283) = "3F" And
strhexbuffN4(284) = "3F" And
strhexbuffN4(285) = "3F" And
strhexbuffN4(286) = "3F" And
strhexbuffN4(287) = "3F" And
strhexbuffN4(288) = "3F" And
strhexbuffN4(289) = "3F" And
strhexbuffN4(290) = "3F" And
strhexbuffN4(291) = "3F" And
strhexbuffN4(292) = "3F" And
strhexbuffN4(293) = "3F" And
strhexbuffN4(294) = "3F" And
strhexbuffN4(295) = "3F" And
strhexbuffN4(296) = "3F" And
strhexbuffN4(297) = "3F" And
strhexbuffN4(298) = "3F" And
strhexbuffN4(299) = "3F" And
strhexbuffN4(300) = "3F" And
strhexbuffN4(301) = "3F" And
strhexbuffN4(302) = "3F" And
strhexbuffN4(303) = "3F" And
strhexbuffN4(304) = "3F" And
strhexbuffN4(305) = "3F" And
strhexbuffN4(306) = "3F" And
strhexbuffN4(307) = "3F" And
strhexbuffN4(308) = "3F" And
strhexbuffN4(309) = "3F" And
strhexbuffN4(310) = "E8" And
strhexbuffN4(311) = "03" And
strhexbuffN4(312) = "00" And
strhexbuffN4(313) = "00" And
strhexbuffN4(314) = "00" And
strhexbuffN4(315) = "00" And
strhexbuffN4(316) = "00" And
strhexbuffN4(317) = "00" And
strhexbuffN4(318) = "00" And
strhexbuffN4(319) = "00" And
strhexbuffN4(320) = "E8" And
strhexbuffN4(321) = "03" And
strhexbuffN4(322) = "00" And
strhexbuffN4(323) = "00" And
strhexbuffN4(324) = "70" And
strhexbuffN4(325) = "17" And
strhexbuffN4(326) = "00" And
strhexbuffN4(327) = "00" And
strhexbuffN4(328) = "03" And
strhexbuffN4(329) = "00" And
strhexbuffN4(330) = "00" And
strhexbuffN4(331) = "00" And
strhexbuffN4(332) = "00" And
strhexbuffN4(333) = "00" And
strhexbuffN4(334) = "00" And
strhexbuffN4(335) = "00" And
strhexbuffN4(336) = "00" And
strhexbuffN4(337) = "00" And
strhexbuffN4(338) = "00" And
strhexbuffN4(339) = "00" And
strhexbuffN4(340) = "00" And
strhexbuffN4(341) = "00" And
strhexbuffN4(342) = "00" And
strhexbuffN4(343) = "00" And
strhexbuffN4(344) = "00" And
strhexbuffN4(345) = "00" And
strhexbuffN4(346) = "00" And
strhexbuffN4(347) = "00" And
strhexbuffN4(348) = "00" And
strhexbuffN4(349) = "00" And
strhexbuffN4(350) = "00" And
strhexbuffN4(351) = "00" And
strhexbuffN4(352) = "00" And
strhexbuffN4(353) = "00" And
strhexbuffN4(354) = "00" And
strhexbuffN4(355) = "00" And
strhexbuffN4(356) = "00" And
strhexbuffN4(357) = "00" And
strhexbuffN4(358) = "00" And
strhexbuffN4(359) = "00" And
strhexbuffN4(360) = "00" And
strhexbuffN4(361) = "00" And
strhexbuffN4(362) = "00" And
strhexbuffN4(363) = "00" And
strhexbuffN4(364) = "00" And
strhexbuffN4(365) = "00" And
strhexbuffN4(366) = "00" And
strhexbuffN4(367) = "00" And
strhexbuffN4(368) = "00" And
strhexbuffN4(369) = "00" And
strhexbuffN4(370) = "00" And
strhexbuffN4(371) = "00" And
strhexbuffN4(372) = "00" And
strhexbuffN4(373) = "00" And
strhexbuffN4(374) = "00" And
strhexbuffN4(375) = "1D" And
strhexbuffN4(376) = "06" And
strhexbuffN4(377) = "01" And
strhexbuffN4(378) = "03" And
strhexbuffN4(379) = "14" And
strhexbuffN4(380) = "14" And
strhexbuffN4(381) = "FF" And
strhexbuffN4(382) = "02" And
strhexbuffN4(383) = "00" And
strhexbuffN4(384) = "04" And
strhexbuffN4(385) = "00" And
strhexbuffN4(386) = "00" And
strhexbuffN4(387) = "00" And
strhexbuffN4(388) = "00" And
strhexbuffN4(389) = "00" And
strhexbuffN4(390) = "01" And
strhexbuffN4(391) = "FF" And
strhexbuffN4(392) = "2C" And
strhexbuffN4(393) = "01" And
strhexbuffN4(394) = "00" And
strhexbuffN4(395) = "00" And
strhexbuffN4(396) = "00" And
strhexbuffN4(397) = "FF" And
strhexbuffN4(398) = "FF" And
strhexbuffN4(399) = "FF" And
strhexbuffN4(400) = "FF" And
strhexbuffN4(401) = "FF" And
strhexbuffN4(402) = "FF" And
strhexbuffN4(403) = "FF" And
strhexbuffN4(404) = "FF" And
strhexbuffN4(405) = "FF" And
strhexbuffN4(406) = "FF" And
strhexbuffN4(407) = "FF" And
strhexbuffN4(408) = "FF" And
strhexbuffN4(409) = "FF" And
strhexbuffN4(410) = "FF" And
strhexbuffN4(411) = "FF" And
strhexbuffN4(412) = "FF" And
strhexbuffN4(413) = "FF" And
strhexbuffN4(414) = "FF" And
strhexbuffN4(415) = "FF" And
strhexbuffN4(416) = "FF" And
strhexbuffN4(417) = "FF" And
strhexbuffN4(418) = "FF" And
strhexbuffN4(419) = "FF" And
strhexbuffN4(420) = "FF" And
strhexbuffN4(421) = "00" And
strhexbuffN4(422) = "FF" And
strhexbuffN4(423) = "FF" And
strhexbuffN4(424) = "FF" And
strhexbuffN4(425) = "FF" And
strhexbuffN4(426) = "FF" And
strhexbuffN4(427) = "FF" And
strhexbuffN4(428) = "FF" And
strhexbuffN4(429) = "FF" And
strhexbuffN4(430) = "FF" And
strhexbuffN4(431) = "00" And
strhexbuffN4(432) = "06" And
strhexbuffN4(433) = "00" And
strhexbuffN4(434) = "FE" And
strhexbuffN4(435) = "00" And
strhexbuffN4(436) = "06" And
strhexbuffN4(437) = "00" And
strhexbuffN4(438) = "FE" And
strhexbuffN4(439) = "00" And
strhexbuffN4(440) = "06" And
strhexbuffN4(441) = "00" And
strhexbuffN4(442) = "FE" And
strhexbuffN4(443) = "00" And
strhexbuffN4(444) = "06" And
strhexbuffN4(445) = "00" And
strhexbuffN4(446) = "FE" And
strhexbuffN4(447) = "00" And
strhexbuffN4(448) = "06" And
strhexbuffN4(449) = "00" And
strhexbuffN4(450) = "FE" And
strhexbuffN4(451) = "00" And
strhexbuffN4(452) = "06" And
strhexbuffN4(453) = "00" And
strhexbuffN4(454) = "FE" And
strhexbuffN4(455) = "00" And
strhexbuffN4(456) = "06" And
strhexbuffN4(457) = "00" And
strhexbuffN4(458) = "FE" And
strhexbuffN4(459) = "00" And
strhexbuffN4(460) = "06" And
strhexbuffN4(461) = "00" And
strhexbuffN4(462) = "FE" And
strhexbuffN4(463) = "00" And
strhexbuffN4(464) = "06" And
strhexbuffN4(465) = "00" And
strhexbuffN4(466) = "FE" And
strhexbuffN4(467) = "00" And
strhexbuffN4(468) = "06" And
strhexbuffN4(469) = "00" And
strhexbuffN4(470) = "FE" And
strhexbuffN4(471) = "00" And
strhexbuffN4(472) = "06" And
strhexbuffN4(473) = "00" And
strhexbuffN4(474) = "FE" And
strhexbuffN4(475) = "00" And
strhexbuffN4(476) = "06" And
strhexbuffN4(477) = "00" And
strhexbuffN4(478) = "FE" And
strhexbuffN4(479) = "00" And
strhexbuffN4(480) = "06" And
strhexbuffN4(481) = "00" And
strhexbuffN4(482) = "FE" And
strhexbuffN4(483) = "00" And
strhexbuffN4(484) = "06" And
strhexbuffN4(485) = "00" And
strhexbuffN4(486) = "FE" And
strhexbuffN4(487) = "00" And
strhexbuffN4(488) = "06" And
strhexbuffN4(489) = "00" And
strhexbuffN4(490) = "FE" And
strhexbuffN4(491) = "00" And
strhexbuffN4(492) = "06" And
strhexbuffN4(493) = "00" And
strhexbuffN4(494) = "FE" And
strhexbuffN4(495) = "00" And
strhexbuffN4(496) = "06" And
strhexbuffN4(497) = "00" And
strhexbuffN4(498) = "FE" And
strhexbuffN4(499) = "00" And
strhexbuffN4(500) = "05" And
strhexbuffN4(501) = "00" And
strhexbuffN4(502) = "00" And
strhexbuffN4(503) = "00" And
strhexbuffN4(504) = "05" And
strhexbuffN4(505) = "00" And
strhexbuffN4(506) = "00" And
strhexbuffN4(507) = "00" And
strhexbuffN4(508) = "F4" And
strhexbuffN4(509) = "F4" And
strhexbuffN4(510) = "F4" And
strhexbuffN4(511) = "F4" Then
            ValidateNVMN4 = True
        Else
            ValidateNVMN4 = False
        End If

    End Sub

    '===== THREADING =====

    Public Sub Bootmode1()
        BootmodeNonPSUN1On()
        Exit Sub
    End Sub

    Public Sub Bootmode2()
        BootmodeNonPSUN2On()
        Exit Sub
    End Sub

    Public Sub Bootmode3()
        BootmodeNonPSUN3On()
        Exit Sub
    End Sub

    Public Sub Bootmode4()
        BootmodeNonPSUN4On()
        Exit Sub
    End Sub

    Public Sub ROM1()
        CheckROMN1()
        Exit Sub
    End Sub

    Public Sub ROM2()
        CheckROMN2()
        Exit Sub
    End Sub

    Public Sub ROM3()
        CheckROMN3()
        Exit Sub
    End Sub

    Public Sub ROM4()
        CheckROMN4()
        Exit Sub
    End Sub

    Public Sub nvmWriteN1()
        UnlockN1()
        SendPALLN1()
        WriteNVMN1()
        Exit Sub
    End Sub

    Public Sub nvmWriteN2()
        UnlockN2()
        SendPALLN2()
        WriteNVMN2()
        Exit Sub
    End Sub

    Public Sub nvmWriteN3()
        UnlockN3()
        SendPALLN3()
        WriteNVMN3()
        Exit Sub
    End Sub

    Public Sub nvmWriteN4()
        UnlockN4()
        SendPALLN4()
        WriteNVMN4()
        Exit Sub
    End Sub

    Public Sub nvmCheckN1()
        CheckNVMN1()
        Exit Sub
    End Sub

    Public Sub nvmCheckN2()
        CheckNVMN2()
        Exit Sub
    End Sub

    Public Sub nvmCheckN3()
        CheckNVMN3()
        Exit Sub
    End Sub

    Public Sub nvmCheckN4()
        CheckNVMN4()
        Exit Sub
    End Sub

    Public Sub nvmCheckDataN1()
        CheckNVMDataN1()
        Exit Sub
    End Sub

    Public Sub nvmCheckDataN2()
        CheckNVMDataN2()
        Exit Sub
    End Sub

    Public Sub nvmCheckDataN3()
        CheckNVMDataN3()
        Exit Sub
    End Sub

    Public Sub nvmCheckDataN4()
        CheckNVMDataN4()
        Exit Sub
    End Sub

    Public Sub CheckLEDN1()
        IgnitionN1On()
        Delay_Ms(100)
        AllLEDN1On()
        Delay_Ms(100)
    End Sub

    Public Sub CheckLEDN2()
        IgnitionN2On()
        Delay_Ms(100)
        AllLEDN2On()
        Delay_Ms(100)
    End Sub

    Public Sub CheckLEDN3()
        IgnitionN3On()
        Delay_Ms(100)
        AllLEDN3On()
        Delay_Ms(100)
    End Sub

    Public Sub CheckLEDN4()
        IgnitionN4On()
        Delay_Ms(100)
        AllLEDN4On()
        Delay_Ms(100)
    End Sub

    '===== MAIN PROGRAM =====

    Public Sub ProcessStep()
        Dim ThreadBootmodeN1 As Thread = New Thread(New ThreadStart(AddressOf Bootmode1))
        Dim ThreadBootmodeN2 As Thread = New Thread(New ThreadStart(AddressOf Bootmode2))
        Dim ThreadBootmodeN3 As Thread = New Thread(New ThreadStart(AddressOf Bootmode3))
        Dim ThreadBootmodeN4 As Thread = New Thread(New ThreadStart(AddressOf Bootmode4))

        Dim ThreadCheckROMN1 As Thread = New Thread(New ThreadStart(AddressOf ROM1))
        Dim ThreadCheckROMN2 As Thread = New Thread(New ThreadStart(AddressOf ROM2))
        Dim ThreadCheckROMN3 As Thread = New Thread(New ThreadStart(AddressOf ROM3))
        Dim ThreadCheckROMN4 As Thread = New Thread(New ThreadStart(AddressOf ROM4))

        Dim ThreadWriteNVMN1 As Thread = New Thread(New ThreadStart(AddressOf nvmWriteN1))
        Dim ThreadWriteNVMN2 As Thread = New Thread(New ThreadStart(AddressOf nvmWriteN2))
        Dim ThreadWriteNVMN3 As Thread = New Thread(New ThreadStart(AddressOf nvmWriteN3))
        Dim ThreadWriteNVMN4 As Thread = New Thread(New ThreadStart(AddressOf nvmWriteN4))

        Dim ThreadCheckNVMN1 As Thread = New Thread(New ThreadStart(AddressOf nvmCheckN1))
        Dim ThreadCheckNVMN2 As Thread = New Thread(New ThreadStart(AddressOf nvmCheckN2))
        Dim ThreadCheckNVMN3 As Thread = New Thread(New ThreadStart(AddressOf nvmCheckN3))
        Dim ThreadCheckNVMN4 As Thread = New Thread(New ThreadStart(AddressOf nvmCheckN4))

        Dim ThreadCheckNVMDataN1 As Thread = New Thread(New ThreadStart(AddressOf nvmCheckDataN1))
        Dim ThreadCheckNVMDataN2 As Thread = New Thread(New ThreadStart(AddressOf nvmCheckDataN2))
        Dim ThreadCheckNVMDataN3 As Thread = New Thread(New ThreadStart(AddressOf nvmCheckDataN3))
        Dim ThreadCheckNVMDataN4 As Thread = New Thread(New ThreadStart(AddressOf nvmCheckDataN4))

        Dim ThreadCheckAllLEDN1 As Thread = New Thread(New ThreadStart(AddressOf CheckLEDN1))
        Dim ThreadCheckAllLEDN2 As Thread = New Thread(New ThreadStart(AddressOf CheckLEDN2))
        Dim ThreadCheckAllLEDN3 As Thread = New Thread(New ThreadStart(AddressOf CheckLEDN3))
        Dim ThreadCheckAllLEDN4 As Thread = New Thread(New ThreadStart(AddressOf CheckLEDN4))

        Select Case StepTest

            Case 0
                RecheckStepN1 = True
                RecheckStepN2 = True
                RecheckStepN3 = True
                RecheckStepN4 = True
                LabelProses.BackColor = Color.Cyan
                LabelProses.Text = ""
                Timer2.Enabled = True
                Timer1.Enabled = False

                PSU15V()
                Delay_Ms(25)

            Case 1
                ProgressBarProcess.Value = StepTest * 1.75
                StepProcess = StepTest
                LabelProses.BackColor = Color.Yellow
                LabelProses.Text = "PROCESS..."
                Interrupt = False
                CountLoop = 0

                If RunningNest1 = True Then
                    OffComPWBAN1()
                    BootmodeN1Off()
                    IgnitionN1Off()
                    AllLEDN1Off()

                    JigN1On()
                    Delay_Ms(100)
                    LabelJudgementN1.BackColor = Color.LightCyan
                    LabelJudgementN1.Text = ""
                    DataGridViewProcessNest1.FirstDisplayedScrollingRowIndex = 1
                End If

                If RunningNest2 = True Then
                    OffComPWBAN2()
                    BootmodeN2Off()
                    IgnitionN2Off()
                    AllLEDN2Off()

                    JigN2On()
                    Delay_Ms(100)
                    LabelJudgementN2.BackColor = Color.LightCyan
                    LabelJudgementN2.Text = ""
                    DataGridViewProcessNest2.FirstDisplayedScrollingRowIndex = 1
                End If

                If RunningNest3 = True Then
                    OffComPWBAN3()
                    BootmodeN3Off()
                    IgnitionN3Off()
                    AllLEDN3Off()

                    JigN3On()
                    Delay_Ms(100)
                    LabelJudgementN3.BackColor = Color.LightCyan
                    LabelJudgementN3.Text = ""
                    DataGridViewProcessNest3.FirstDisplayedScrollingRowIndex = 1
                End If

                If RunningNest4 = True Then
                    OffComPWBAN4()
                    BootmodeN4Off()
                    IgnitionN4Off()
                    AllLEDN4Off()

                    JigN4On()
                    Delay_Ms(100)
                    LabelJudgementN4.BackColor = Color.LightCyan
                    LabelJudgementN4.Text = ""
                    DataGridViewProcessNest4.FirstDisplayedScrollingRowIndex = 1
                End If

                StepTest = 2

            '===== CHECK ROM ====='

            Case 2
                ProgressBarProcess.Value = StepTest * 1.75
                StepProcess = StepTest

                If RunningNest1 = False Then
                    ProcessN1 = False
                End If

                If RunningNest2 = False Then
                    ProcessN2 = False
                End If

                If RunningNest3 = False Then
                    ProcessN3 = False
                End If

                If RunningNest4 = False Then
                    ProcessN4 = False
                End If

                If RunningNest1 = True And RunningNest2 = True And RunningNest3 = True And RunningNest4 = True Then

                    '===== CHECK ROM ALL NEST ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN1On()
                        BootmodeNonPSUN2On()
                        BootmodeNonPSUN3On()
                        BootmodeNonPSUN4On()
                        Delay_Ms(1000)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN2On()
                            BootmodeAN3On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN2Off()
                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            BootmodeAN3On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN3On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN2On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN2On()
                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN3On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN2Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN2Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN1Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    ElseIf CountLoop = 2 Then

                        If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN2On()
                            BootmodeBN3On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN2Off()
                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            BootmodeBN3On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN3On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN2On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN2On()
                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN3On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN2Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN2Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN1Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    End If

                    ThreadCheckROMN1.Start()
                    ThreadCheckROMN1.Join()

                    ThreadCheckROMN2.Start()
                    ThreadCheckROMN2.Join()

                    ThreadCheckROMN3.Start()
                    ThreadCheckROMN3.Join()

                    ThreadCheckROMN4.Start()
                    ThreadCheckROMN4.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(0) & "." & strhexbuffN1(1)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(0) & "." & strhexbuffN2(1)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(0) & "." & strhexbuffN3(1)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(0) & "." & strhexbuffN4(1)

                    If strhexbuffN1(0) = CharROM(1) And strhexbuffN1(1) = CharROM(2) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN1()
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 1|F|CHECK ROM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False

                    End If

                    If strhexbuffN2(0) = CharROM(1) And strhexbuffN2(1) = CharROM(2) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN2()
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 2|F|CHECK ROM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False

                    End If

                    If strhexbuffN3(0) = CharROM(1) And strhexbuffN3(1) = CharROM(2) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN3()
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 3|F|CHECK ROM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False

                    End If

                    If strhexbuffN4(0) = CharROM(1) And strhexbuffN4(1) = CharROM(2) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN4()
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 4|F|CHECK ROM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest2NG()
                            Nest3NG()
                            Nest4NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            Nest3NG()
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest3NG()
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest2NG()
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest2NG()
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest3NG()
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    Else
                        StepTest += 1
                    End If

                ElseIf RunningNest1 = True And RunningNest2 = True And RunningNest3 = True And RunningNest4 = False Then

                    '===== CHECK ROM NEST 1, NEST 2, NEST 3 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN1On()
                        BootmodeNonPSUN2On()
                        BootmodeNonPSUN3On()
                        Delay_Ms(1000)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN2On()
                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False Then

                            BootmodeN2Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False Then

                            BootmodeN1Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    ElseIf CountLoop = 2 Then

                        If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN2On()
                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False Then

                            BootmodeN2Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False Then

                            BootmodeN1Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    End If

                    ThreadCheckROMN1.Start()
                    ThreadCheckROMN1.Join()

                    ThreadCheckROMN2.Start()
                    ThreadCheckROMN2.Join()

                    ThreadCheckROMN3.Start()
                    ThreadCheckROMN3.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(0) & "." & strhexbuffN1(1)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(0) & "." & strhexbuffN2(1)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(0) & "." & strhexbuffN3(1)

                    If strhexbuffN1(0) = CharROM(1) And strhexbuffN1(1) = CharROM(2) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN1()
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 1|F|CHECK ROM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False

                    End If

                    If strhexbuffN2(0) = CharROM(1) And strhexbuffN2(1) = CharROM(2) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN2()
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 2|F|CHECK ROM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False

                    End If

                    If strhexbuffN3(0) = CharROM(1) And strhexbuffN3(1) = CharROM(2) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN3()
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 3|F|CHECK ROM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest2NG()
                            Nest3NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest2NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    Else
                        StepTest += 1
                    End If

                ElseIf RunningNest1 = True And RunningNest2 = True And RunningNest3 = False And RunningNest4 = True Then

                    '===== CHECK ROM NEST 1, NEST 2, NEST 4 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN1On()
                        BootmodeNonPSUN2On()
                        BootmodeNonPSUN4On()
                        Delay_Ms(1000)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        If ProcessN1 = False And ProcessN2 = False And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN2On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN4 = False Then

                            BootmodeN2Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN4 = True Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN4 = True Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN4 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    ElseIf CountLoop = 2 Then

                        If ProcessN1 = False And ProcessN2 = False And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN2On()
                            BootmodeBN4On()

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN4 = False Then

                            BootmodeN2Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN4 = True Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN4 = True Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN4 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    End If

                    ThreadCheckROMN1.Start()
                    ThreadCheckROMN1.Join()

                    ThreadCheckROMN2.Start()
                    ThreadCheckROMN2.Join()

                    ThreadCheckROMN4.Start()
                    ThreadCheckROMN4.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(0) & "." & strhexbuffN1(1)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(0) & "." & strhexbuffN2(1)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(0) & "." & strhexbuffN4(1)

                    If strhexbuffN1(0) = CharROM(1) And strhexbuffN1(1) = CharROM(2) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN1()
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 1|F|CHECK ROM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False

                    End If

                    If strhexbuffN2(0) = CharROM(1) And strhexbuffN2(1) = CharROM(2) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN2()
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 2|F|CHECK ROM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False

                    End If

                    If strhexbuffN4(0) = CharROM(1) And strhexbuffN4(1) = CharROM(2) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN4()
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 4|F|CHECK ROM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest2NG()
                            Nest4NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest2NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    Else
                        StepTest += 1
                    End If

                ElseIf RunningNest1 = True And RunningNest2 = False And RunningNest3 = True And RunningNest4 = True Then

                    '===== CHECK ROM NEST 1, NEST 3, NEST 4 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN1On()
                        BootmodeNonPSUN3On()
                        BootmodeNonPSUN4On()
                        Delay_Ms(1000)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        If ProcessN1 = False And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN3On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN3On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN1Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN3 = True And ProcessN4 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    ElseIf CountLoop = 2 Then

                        If ProcessN1 = False And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN3On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN3On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN1Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN3 = True And ProcessN4 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    End If

                    ThreadCheckROMN1.Start()
                    ThreadCheckROMN1.Join()

                    ThreadCheckROMN3.Start()
                    ThreadCheckROMN3.Join()

                    ThreadCheckROMN4.Start()
                    ThreadCheckROMN4.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(0) & "." & strhexbuffN1(1)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(0) & "." & strhexbuffN3(1)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(0) & "." & strhexbuffN4(1)

                    If strhexbuffN1(0) = CharROM(1) And strhexbuffN1(1) = CharROM(2) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN1()
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 1|F|CHECK ROM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False

                    End If

                    If strhexbuffN3(0) = CharROM(1) And strhexbuffN3(1) = CharROM(2) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN3()
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 3|F|CHECK ROM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False

                    End If

                    If strhexbuffN4(0) = CharROM(1) And strhexbuffN4(1) = CharROM(2) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN4()
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 4|F|CHECK ROM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False

                    End If

                    If ProcessN1 = False And ProcessN3 = False And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest3NG()
                            Nest4NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN3 = False And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest3NG()
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN3 = True And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN3 = False And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN3 = True And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN3 = False And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN3 = True And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    Else
                        StepTest += 1
                    End If

                ElseIf RunningNest1 = False And RunningNest2 = True And RunningNest3 = True And RunningNest4 = True Then

                    '===== CHECK ROM NEST 2, NEST 3, NEST 4 ====='

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN2On()
                        BootmodeNonPSUN3On()
                        BootmodeNonPSUN4On()
                        Delay_Ms(1000)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        If ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN2Off()
                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            BootmodeAN3On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN3On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN2Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN2Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    ElseIf CountLoop = 2 Then

                        If ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN2Off()
                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            BootmodeBN3On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN3On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN2Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN2Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    End If

                    ThreadCheckROMN2.Start()
                    ThreadCheckROMN2.Join()

                    ThreadCheckROMN3.Start()
                    ThreadCheckROMN3.Join()

                    ThreadCheckROMN4.Start()
                    ThreadCheckROMN4.Join()

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(0) & "." & strhexbuffN2(1)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(0) & "." & strhexbuffN3(1)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(0) & "." & strhexbuffN4(1)

                    If strhexbuffN2(0) = CharROM(1) And strhexbuffN2(1) = CharROM(2) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN2()
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 2|F|CHECK ROM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False

                    End If

                    If strhexbuffN3(0) = CharROM(1) And strhexbuffN3(1) = CharROM(2) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN3()
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 3|F|CHECK ROM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False

                    End If

                    If strhexbuffN4(0) = CharROM(1) And strhexbuffN4(1) = CharROM(2) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN4()
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 4|F|CHECK ROM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False

                    End If

                    If ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            Nest3NG()
                            Nest4NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest3NG()
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                        CountLoop += 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    Else
                        StepTest += 1
                    End If

                ElseIf RunningNest1 = True And RunningNest2 = True And RunningNest3 = False And RunningNest4 = False Then

                    '===== CHECK ROM NEST 1, NEST 2 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN1On()
                        BootmodeNonPSUN2On()
                        Delay_Ms(1000)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        If ProcessN1 = False And ProcessN2 = False Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeAN1On()

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    ElseIf CountLoop = 2 Then

                        If ProcessN1 = False And ProcessN2 = False Then

                            BootmodeN1Off()
                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN2 = False Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN2 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    End If

                    ThreadCheckROMN1.Start()
                    ThreadCheckROMN1.Join()

                    ThreadCheckROMN2.Start()
                    ThreadCheckROMN2.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(0) & "." & strhexbuffN1(1)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(0) & "." & strhexbuffN2(1)

                    If strhexbuffN1(0) = CharROM(1) And strhexbuffN1(1) = CharROM(2) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN1()
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 1|F|CHECK ROM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False

                    End If

                    If strhexbuffN2(0) = CharROM(1) And strhexbuffN2(1) = CharROM(2) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN2()
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 2|F|CHECK ROM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False

                    End If

                    If ProcessN1 = False And ProcessN2 = False Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest2NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN2 = False Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN2 = True Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    Else
                        StepTest += 1
                    End If

                ElseIf RunningNest1 = True And RunningNest2 = False And RunningNest3 = True And RunningNest4 = False Then

                    '===== CHECK ROM NEST 1, NEST 3 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN1On()
                        BootmodeNonPSUN3On()
                        Delay_Ms(1000)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        If ProcessN1 = False And ProcessN3 = False Then

                            BootmodeN1Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN3 = False Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN3 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    ElseIf CountLoop = 2 Then

                        If ProcessN1 = False And ProcessN3 = False Then

                            BootmodeN1Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN3 = False Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V() 
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN3 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    End If

                    ThreadCheckROMN1.Start()
                    ThreadCheckROMN1.Join()

                    ThreadCheckROMN3.Start()
                    ThreadCheckROMN3.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(0) & "." & strhexbuffN1(1)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(0) & "." & strhexbuffN3(1)

                    If strhexbuffN1(0) = CharROM(1) And strhexbuffN1(1) = CharROM(2) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN1()
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 1|F|CHECK ROM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False

                    End If

                    If strhexbuffN3(0) = CharROM(1) And strhexbuffN3(1) = CharROM(2) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN3()
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 3|F|CHECK ROM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False

                    End If

                    If ProcessN1 = False And ProcessN3 = False Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest3NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN3 = False Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN3 = True Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    Else
                        StepTest += 1
                    End If

                ElseIf RunningNest1 = True And RunningNest2 = False And RunningNest3 = False And RunningNest4 = True Then

                    '===== CHECK ROM NEST 1, NEST 4 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN1On()
                        BootmodeNonPSUN4On()
                        Delay_Ms(1000)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        If ProcessN1 = False And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN4 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeAN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    ElseIf CountLoop = 2 Then

                        If ProcessN1 = False And ProcessN4 = False Then

                            BootmodeN1Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN1 = False And ProcessN4 = True Then

                            BootmodeN1Off()
                            Delay_Ms(200)

                            BootmodeBN1On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    End If

                    ThreadCheckROMN1.Start()
                    ThreadCheckROMN1.Join()

                    ThreadCheckROMN4.Start()
                    ThreadCheckROMN4.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(0) & "." & strhexbuffN1(1)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(0) & "." & strhexbuffN4(1)

                    If strhexbuffN1(0) = CharROM(1) And strhexbuffN1(1) = CharROM(2) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN1()
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 1|F|CHECK ROM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False

                    End If

                    If strhexbuffN4(0) = CharROM(1) And strhexbuffN4(1) = CharROM(2) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN4()
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 4|F|CHECK ROM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False

                    End If

                    If ProcessN1 = False And ProcessN4 = False Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            Nest4NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = True And ProcessN4 = False Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN1 = False And ProcessN4 = True Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest1NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    Else
                        StepTest += 1
                    End If

                ElseIf RunningNest1 = False And RunningNest2 = True And RunningNest3 = True And RunningNest4 = False Then

                    '===== CHECK ROM NEST 2, NEST 3 ====='

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN2On()
                        BootmodeNonPSUN3On()
                        Delay_Ms(1000)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        If ProcessN2 = False And ProcessN3 = False Then

                            BootmodeN2Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = True And ProcessN3 = False Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = False And ProcessN3 = True Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    ElseIf CountLoop = 2 Then

                        If ProcessN2 = False And ProcessN3 = False Then

                            BootmodeN2Off()
                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = True And ProcessN3 = False Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = False And ProcessN3 = True Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    End If

                    ThreadCheckROMN2.Start()
                    ThreadCheckROMN2.Join()

                    ThreadCheckROMN3.Start()
                    ThreadCheckROMN3.Join()

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(0) & "." & strhexbuffN2(1)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(0) & "." & strhexbuffN3(1)

                    If strhexbuffN2(0) = CharROM(1) And strhexbuffN2(1) = CharROM(2) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN2()
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 2|F|CHECK ROM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False

                    End If

                    If strhexbuffN3(0) = CharROM(1) And strhexbuffN3(1) = CharROM(2) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN3()
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 3|F|CHECK ROM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False

                    End If

                    If ProcessN2 = False And ProcessN3 = False Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            Nest3NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN2 = True And ProcessN3 = False Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN2 = False And ProcessN3 = True Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    Else
                        StepTest += 1
                    End If

                ElseIf RunningNest1 = False And RunningNest2 = True And RunningNest3 = False And RunningNest4 = True Then

                    '===== CHECK ROM NEST 2, NEST 4 ====='

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN2On()
                        BootmodeNonPSUN4On()
                        Delay_Ms(1000)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        If ProcessN2 = False And ProcessN4 = False Then

                            BootmodeN2Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = False And ProcessN4 = True Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeAN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    ElseIf CountLoop = 2 Then

                        If ProcessN2 = False And ProcessN4 = False Then

                            BootmodeN2Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN2 = False And ProcessN4 = True Then

                            BootmodeN2Off()
                            Delay_Ms(200)

                            BootmodeBN2On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    End If

                    ThreadCheckROMN2.Start()
                    ThreadCheckROMN2.Join()

                    ThreadCheckROMN4.Start()
                    ThreadCheckROMN4.Join()

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(0) & "." & strhexbuffN2(1)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(0) & "." & strhexbuffN4(1)

                    If strhexbuffN2(0) = CharROM(1) And strhexbuffN2(1) = CharROM(2) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN2()
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 2|F|CHECK ROM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False

                    End If

                    If strhexbuffN4(0) = CharROM(1) And strhexbuffN4(1) = CharROM(2) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN4()
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 4|F|CHECK ROM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False

                    End If

                    If ProcessN2 = False And ProcessN4 = False Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            Nest4NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN2 = True And ProcessN4 = False Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN2 = False And ProcessN4 = True Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest2NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    Else
                        StepTest += 1
                    End If

                ElseIf RunningNest1 = False And RunningNest2 = False And RunningNest3 = True And RunningNest4 = True Then

                    '===== CHECK ROM NEST 3, NEST 4 ====='

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN3On()
                        BootmodeNonPSUN4On()
                        Delay_Ms(1000)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        If ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN3On()
                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeAN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeAN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    ElseIf CountLoop = 2 Then

                        If ProcessN3 = False And ProcessN4 = False Then

                            BootmodeN3Off()
                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN3On()
                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN3 = True And ProcessN4 = False Then

                            BootmodeN4Off()
                            Delay_Ms(200)

                            BootmodeBN4On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        ElseIf ProcessN3 = False And ProcessN4 = True Then

                            BootmodeN3Off()
                            Delay_Ms(200)

                            BootmodeBN3On()
                            Delay_Ms(1000)

                            'If CheckBoxAlparteIC100.Checked = True Then
                            '    PSU18V()
                            '    Delay_Ms(15000)
                            'Else
                            '    Delay_Ms(200)
                            'End If

                        End If

                    End If

                    ThreadCheckROMN3.Start()
                    ThreadCheckROMN3.Join()

                    ThreadCheckROMN4.Start()
                    ThreadCheckROMN4.Join()

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(0) & "." & strhexbuffN3(1)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(0) & "." & strhexbuffN4(1)

                    If strhexbuffN3(0) = CharROM(1) And strhexbuffN3(1) = CharROM(2) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN3()
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 3|F|CHECK ROM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False

                    End If

                    If strhexbuffN4(0) = CharROM(1) And strhexbuffN4(1) = CharROM(2) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN4()
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK ROM NEST 4|F|CHECK ROM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False

                    End If

                    If ProcessN3 = False And ProcessN4 = False Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest3NG()
                            Nest4NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN3 = True And ProcessN4 = False Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest4NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    ElseIf ProcessN3 = False And ProcessN4 = True Then

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            Nest3NG()
                            StepTest += 1
                        Else
                            StepTest = 2
                        End If

                    Else
                        StepTest += 1
                    End If

                ElseIf RunningNest1 = True And RunningNest2 = False And RunningNest3 = False And RunningNest4 = False Then

                    '===== CHECK ROM NEST 1 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN1On()
                        Delay_Ms(500)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        BootmodeN1Off()
                        Delay_Ms(200)

                        BootmodeAN1On()
                        Delay_Ms(500)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 2 Then

                        BootmodeN1Off()
                        Delay_Ms(200)

                        BootmodeBN1On()
                        Delay_Ms(500)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    End If

                    CheckROMN1()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(0) & "." & strhexbuffN1(1)

                    If strhexbuffN1(0) = CharROM(1) And strhexbuffN1(1) = CharROM(2) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN1()
                        ProcessN1 = True
                        StepTest += 1

                    Else

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            NGType = "|*|FUN|*|1|1|CHECK ROM NEST 1|F|CHECK ROM NEST 1|||"
                            DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                            DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                            ProcessN1 = False
                            Nest1NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    End If

                ElseIf RunningNest1 = False And RunningNest2 = True And RunningNest3 = False And RunningNest4 = False Then

                    '===== CHECK ROM NEST 2 ====='

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN2On()
                        Delay_Ms(500)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        BootmodeN2Off()
                        Delay_Ms(200)

                        BootmodeAN2On()
                        Delay_Ms(500)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 2 Then

                        BootmodeN2Off()
                        Delay_Ms(200)

                        BootmodeBN2On()
                        Delay_Ms(500)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    End If

                    CheckROMN2()

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(0) & "." & strhexbuffN2(1)

                    If strhexbuffN2(0) = CharROM(1) And strhexbuffN2(1) = CharROM(2) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN2()
                        ProcessN2 = True
                        StepTest += 1

                    Else

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            NGType = "|*|FUN|*|1|1|CHECK ROM NEST 2|F|CHECK ROM NEST 2|||"
                            DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                            DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                            ProcessN2 = False
                            Nest2NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    End If

                ElseIf RunningNest1 = False And RunningNest2 = False And RunningNest3 = True And RunningNest4 = False Then

                    '===== CHECK ROM NEST 3 ====='

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN3On()
                        Delay_Ms(500)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        BootmodeN3Off()
                        Delay_Ms(200)

                        BootmodeAN3On()
                        Delay_Ms(500)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 2 Then

                        BootmodeN3Off()
                        Delay_Ms(200)

                        BootmodeBN3On()
                        Delay_Ms(500)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    End If

                    CheckROMN3()

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(0) & "." & strhexbuffN3(1)

                    If strhexbuffN3(0) = CharROM(1) And strhexbuffN3(1) = CharROM(2) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN3()
                        ProcessN3 = True
                        StepTest += 1

                    Else

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            NGType = "|*|FUN|*|1|1|CHECK ROM NEST 3|F|CHECK ROM NEST 3|||"
                            DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                            DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                            ProcessN3 = False
                            Nest3NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    End If

                ElseIf RunningNest1 = False And RunningNest2 = False And RunningNest3 = False And RunningNest4 = True Then

                    '===== CHECK ROM NEST 4 ====='

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check ROM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharROM(1) & "." & CharROM(2)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharROM(1) & "." & CharROM(2)

                    If CountLoop = 0 Then

                        BootmodeNonPSUN4On()
                        Delay_Ms(500)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 1 Then

                        BootmodeN4Off()
                        Delay_Ms(200)

                        BootmodeAN4On()
                        Delay_Ms(500)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    ElseIf CountLoop = 2 Then

                        BootmodeN4Off()
                        Delay_Ms(200)

                        BootmodeBN4On()
                        Delay_Ms(500)

                        'If CheckBoxAlparteIC100.Checked = True Then
                        '    PSU18V()
                        '    Delay_Ms(15000)
                        'Else
                        '    Delay_Ms(200)
                        'End If

                    End If

                    CheckROMN4()

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(0) & "." & strhexbuffN4(1)

                    If strhexbuffN4(0) = CharROM(1) And strhexbuffN4(1) = CharROM(2) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        OffComPWBAN4()
                        ProcessN4 = True
                        StepTest += 1

                    Else

                        CountLoop = CountLoop + 1
                        If CountLoop = 2 Then
                            NGType = "|*|FUN|*|1|1|CHECK ROM NEST 4|F|CHECK ROM NEST 4|||"
                            DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                            DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                            ProcessN4 = False
                            Nest4NG()
                            StepTest = 123
                        Else
                            StepTest = 2
                        End If

                    End If

                ElseIf RunningNest1 = False And RunningNest2 = False And RunningNest3 = False And RunningNest4 = False Then

                    StepTest = 123

                End If

            '===== WRITE NVM ====='

            Case 3
                ProgressBarProcess.Value = StepTest * 1.75
                StepProcess = StepTest

                If CheckBoxAlparteIC100.Checked = True Then
                    PSU18V()
                    Delay_Ms(15000)
                Else
                    Delay_Ms(200)
                End If

                If CheckBoxNVMOffline.Checked = True Then

                    If ProcessN1 = True And ProcessN2 = True And ProcessN3 = True And ProcessN4 = True Then

                        '===== WRITE NVM ALL NEST ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        ThreadWriteNVMN1.Start()
                        ThreadWriteNVMN1.Join()

                        ThreadWriteNVMN2.Start()
                        ThreadWriteNVMN2.Join()

                        ThreadWriteNVMN3.Start()
                        ThreadWriteNVMN3.Join()

                        ThreadWriteNVMN4.Start()
                        ThreadWriteNVMN4.Join()

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN2 = True
                        ProcessN3 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                        '===== WRITE NVM NEST 1, NEST 2, NEST 3 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"

                        ThreadWriteNVMN1.Start()
                        ThreadWriteNVMN1.Join()

                        ThreadWriteNVMN2.Start()
                        ThreadWriteNVMN2.Join()

                        ThreadWriteNVMN3.Start()
                        ThreadWriteNVMN3.Join()

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN2 = True
                        ProcessN3 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                        '===== WRITE NVM NEST 1, NEST 2, NEST 4 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        ThreadWriteNVMN1.Start()
                        ThreadWriteNVMN1.Join()

                        ThreadWriteNVMN2.Start()
                        ThreadWriteNVMN2.Join()

                        ThreadWriteNVMN4.Start()
                        ThreadWriteNVMN4.Join()

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN2 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                        '===== WRITE NVM NEST 1, NEST 3, NEST 4 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        ThreadWriteNVMN1.Start()
                        ThreadWriteNVMN1.Join()

                        ThreadWriteNVMN3.Start()
                        ThreadWriteNVMN3.Join()

                        ThreadWriteNVMN4.Start()
                        ThreadWriteNVMN4.Join()

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN3 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = True Then

                        '===== WRITE NVM NEST 2, NEST 3, NEST 4 ====='

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        ThreadWriteNVMN2.Start()
                        ThreadWriteNVMN2.Join()

                        ThreadWriteNVMN3.Start()
                        ThreadWriteNVMN3.Join()

                        ThreadWriteNVMN4.Start()
                        ThreadWriteNVMN4.Join()

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN2 = True
                        ProcessN3 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                        '===== WRITE NVM NEST 1, NEST 2 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"

                        ThreadWriteNVMN1.Start()
                        ThreadWriteNVMN1.Join()

                        ThreadWriteNVMN2.Start()
                        ThreadWriteNVMN2.Join()

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN2 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                        '===== WRITE NVM NEST 1, NEST 3 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"

                        ThreadWriteNVMN1.Start()
                        ThreadWriteNVMN1.Join()

                        ThreadWriteNVMN3.Start()
                        ThreadWriteNVMN3.Join()

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN3 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                        '===== WRITE NVM NEST 1, NEST 4 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        ThreadWriteNVMN1.Start()
                        ThreadWriteNVMN1.Join()

                        ThreadWriteNVMN4.Start()
                        ThreadWriteNVMN4.Join()

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                        '===== WRITE NVM NEST 2, NEST 3 ====='

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"

                        ThreadWriteNVMN2.Start()
                        ThreadWriteNVMN2.Join()

                        ThreadWriteNVMN3.Start()
                        ThreadWriteNVMN3.Join()

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN2 = True
                        ProcessN3 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                        '===== WRITE NVM NEST 2, NEST 4 ====='

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        ThreadWriteNVMN2.Start()
                        ThreadWriteNVMN2.Join()

                        ThreadWriteNVMN4.Start()
                        ThreadWriteNVMN4.Join()

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN2 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                        '===== WRITE NVM NEST 3, NEST 4 ====='

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        ThreadWriteNVMN3.Start()
                        ThreadWriteNVMN3.Join()

                        ThreadWriteNVMN4.Start()
                        ThreadWriteNVMN4.Join()

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN3 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                        '===== WRITE NVM NEST 1 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"

                        UnlockN1()
                        SendPALLN1()
                        WriteNVMN1()

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                        '===== WRITE NVM NEST 2 ====='

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"

                        UnlockN2()
                        SendPALLN2()
                        WriteNVMN2()

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN2 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                        '===== WRITE NVM NEST 3 ====='

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"

                        UnlockN3()
                        SendPALLN3()
                        WriteNVMN3()

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN3 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                        '===== WRITE NVM NEST 4 ====='

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        UnlockN4()
                        SendPALLN4()
                        WriteNVMN4()

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "FINISH"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                        StepTest = 123

                    End If

                Else

                    If ProcessN1 = True And ProcessN2 = True And ProcessN3 = True And ProcessN4 = True Then

                        '===== SKIP WRITE NVM ALL NEST ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN2 = True
                        ProcessN3 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                        '===== SKIP WRITE NVM NEST 1, NEST 2, NEST 3 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN2 = True
                        ProcessN3 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                        '===== SKIP WRITE NVM NEST 1, NEST 2, NEST 4 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN2 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                        '===== SKIP WRITE NVM NEST 1, NEST 3, NEST 4 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN3 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = True Then

                        '===== SKIP WRITE NVM NEST 2, NEST 3, NEST 4 ====='

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN2 = True
                        ProcessN3 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                        '===== SKIP WRITE NVM NEST 1, NEST 2 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN2 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                        '===== SKIP WRITE NVM NEST 1, NEST 3 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN3 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                        '===== SKIP WRITE NVM NEST 1, NEST 4 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                        '===== SKIP WRITE NVM NEST 2, NEST 3 ====='

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN2 = True
                        ProcessN3 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                        '===== SKIP WRITE NVM NEST 2, NEST 4 ====='

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN2 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                        '===== SKIP WRITE NVM NEST 3, NEST 4 ====='

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN3 = True
                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                        '===== SKIP WRITE NVM NEST 1 ====='

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N1"

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN1 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                        '===== SKIP WRITE NVM NEST 2 ====='

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N2"

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN2 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                        '===== SKIP WRITE NVM NEST 3 ====='

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N3"

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN3 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                        '===== SKIP WRITE NVM NEST 4 ====='

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Write NVM N4"

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.Gray
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "SKIPPED"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.Aqua
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow

                        ProcessN4 = True

                        StepTest += 1

                    ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    End If

                End If

            '===== CHECK NVM ====='

            Case 4
                ProgressBarProcess.Value = StepTest * 1.75
                StepProcess = StepTest

                If ProcessN1 = True And ProcessN2 = True And ProcessN3 = True And ProcessN4 = True Then

                    '===== CHECK NVM ALL NEST ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    ThreadCheckNVMN1.Start()
                    ThreadCheckNVMN1.Join()

                    ThreadCheckNVMN2.Start()
                    ThreadCheckNVMN2.Join()

                    ThreadCheckNVMN3.Start()
                    ThreadCheckNVMN3.Join()

                    ThreadCheckNVMN4.Start()
                    ThreadCheckNVMN4.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(4) & "." & strhexbuffN1(5) & "." & strhexbuffN1(6)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(4) & "." & strhexbuffN2(5) & "." & strhexbuffN2(6)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(4) & "." & strhexbuffN3(5) & "." & strhexbuffN3(6)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(4) & "." & strhexbuffN4(5) & "." & strhexbuffN4(6)

                    If strhexbuffN1(4) = CharNVM(1) And strhexbuffN1(5) = CharNVM(2) And strhexbuffN1(6) = CharNVM(3) Then
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 1|F|CHECK NVM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()
                    End If

                    If strhexbuffN2(4) = CharNVM(1) And strhexbuffN2(5) = CharNVM(2) And strhexbuffN2(6) = CharNVM(3) Then
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 2|F|CHECK NVM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()
                    End If

                    If strhexbuffN3(4) = CharNVM(1) And strhexbuffN3(5) = CharNVM(2) And strhexbuffN3(6) = CharNVM(3) Then
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 3|F|CHECK NVM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()
                    End If

                    If strhexbuffN4(4) = CharNVM(1) And strhexbuffN4(5) = CharNVM(2) And strhexbuffN4(6) = CharNVM(3) Then
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 4|F|CHECK NVM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()
                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                    '===== CHECK NVM NEST 1, NEST 2, NEST 3 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    ThreadCheckNVMN1.Start()
                    ThreadCheckNVMN1.Join()

                    ThreadCheckNVMN2.Start()
                    ThreadCheckNVMN2.Join()

                    ThreadCheckNVMN3.Start()
                    ThreadCheckNVMN3.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(4) & "." & strhexbuffN1(5) & "." & strhexbuffN1(6)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(4) & "." & strhexbuffN2(5) & "." & strhexbuffN2(6)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(4) & "." & strhexbuffN3(5) & "." & strhexbuffN3(6)

                    If strhexbuffN1(4) = CharNVM(1) And strhexbuffN1(5) = CharNVM(2) And strhexbuffN1(6) = CharNVM(3) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 1|F|CHECK NVM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If strhexbuffN2(4) = CharNVM(1) And strhexbuffN2(5) = CharNVM(2) And strhexbuffN2(6) = CharNVM(3) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 2|F|CHECK NVM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If strhexbuffN3(4) = CharNVM(1) And strhexbuffN3(5) = CharNVM(2) And strhexbuffN3(6) = CharNVM(3) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 3|F|CHECK NVM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                    '===== CHECK NVM NEST 1, NEST 2, NEST 4 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    ThreadCheckNVMN1.Start()
                    ThreadCheckNVMN1.Join()

                    ThreadCheckNVMN2.Start()
                    ThreadCheckNVMN2.Join()

                    ThreadCheckNVMN4.Start()
                    ThreadCheckNVMN4.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(4) & "." & strhexbuffN1(5) & "." & strhexbuffN1(6)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(4) & "." & strhexbuffN2(5) & "." & strhexbuffN2(6)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(4) & "." & strhexbuffN4(5) & "." & strhexbuffN4(6)

                    If strhexbuffN1(4) = CharNVM(1) And strhexbuffN1(5) = CharNVM(2) And strhexbuffN1(6) = CharNVM(3) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 1|F|CHECK NVM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If strhexbuffN2(4) = CharNVM(1) And strhexbuffN2(5) = CharNVM(2) And strhexbuffN2(6) = CharNVM(3) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 2|F|CHECK NVM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If strhexbuffN4(4) = CharNVM(1) And strhexbuffN4(5) = CharNVM(2) And strhexbuffN4(6) = CharNVM(3) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 4|F|CHECK NVM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                    '===== CHECK NVM NEST 1, NEST 3, NEST 4 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    ThreadCheckNVMN1.Start()
                    ThreadCheckNVMN1.Join()

                    ThreadCheckNVMN3.Start()
                    ThreadCheckNVMN3.Join()

                    ThreadCheckNVMN4.Start()
                    ThreadCheckNVMN4.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(4) & "." & strhexbuffN1(5) & "." & strhexbuffN1(6)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(4) & "." & strhexbuffN3(5) & "." & strhexbuffN3(6)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(4) & "." & strhexbuffN4(5) & "." & strhexbuffN4(6)

                    If strhexbuffN1(4) = CharNVM(1) And strhexbuffN1(5) = CharNVM(2) And strhexbuffN1(6) = CharNVM(3) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 1|F|CHECK NVM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If strhexbuffN3(4) = CharNVM(1) And strhexbuffN3(5) = CharNVM(2) And strhexbuffN3(6) = CharNVM(3) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 3|F|CHECK NVM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If strhexbuffN4(4) = CharNVM(1) And strhexbuffN4(5) = CharNVM(2) And strhexbuffN4(6) = CharNVM(3) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 4|F|CHECK NVM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = True Then

                    '===== CHECK NVM NEST 2, NEST 3, NEST 4 ====='

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    ThreadCheckNVMN2.Start()
                    ThreadCheckNVMN2.Join()

                    ThreadCheckNVMN3.Start()
                    ThreadCheckNVMN3.Join()

                    ThreadCheckNVMN4.Start()
                    ThreadCheckNVMN4.Join()

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(4) & "." & strhexbuffN2(5) & "." & strhexbuffN2(6)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(4) & "." & strhexbuffN3(5) & "." & strhexbuffN3(6)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(4) & "." & strhexbuffN4(5) & "." & strhexbuffN4(6)

                    If strhexbuffN2(4) = CharNVM(1) And strhexbuffN2(5) = CharNVM(2) And strhexbuffN2(6) = CharNVM(3) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 2|F|CHECK NVM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If strhexbuffN3(4) = CharNVM(1) And strhexbuffN3(5) = CharNVM(2) And strhexbuffN3(6) = CharNVM(3) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 3|F|CHECK NVM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If strhexbuffN4(4) = CharNVM(1) And strhexbuffN4(5) = CharNVM(2) And strhexbuffN4(6) = CharNVM(3) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 4|F|CHECK NVM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                    '===== CHECK NVM NEST 1, NEST 2 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    ThreadCheckNVMN1.Start()
                    ThreadCheckNVMN1.Join()

                    ThreadCheckNVMN2.Start()
                    ThreadCheckNVMN2.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(4) & "." & strhexbuffN1(5) & "." & strhexbuffN1(6)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(4) & "." & strhexbuffN2(5) & "." & strhexbuffN2(6)

                    If strhexbuffN1(4) = CharNVM(1) And strhexbuffN1(5) = CharNVM(2) And strhexbuffN1(6) = CharNVM(3) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 1|F|CHECK NVM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If strhexbuffN2(4) = CharNVM(1) And strhexbuffN2(5) = CharNVM(2) And strhexbuffN2(6) = CharNVM(3) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 2|F|CHECK NVM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                    '===== CHECK NVM NEST 1, NEST 3 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    ThreadCheckNVMN1.Start()
                    ThreadCheckNVMN1.Join()

                    ThreadCheckNVMN3.Start()
                    ThreadCheckNVMN3.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(4) & "." & strhexbuffN1(5) & "." & strhexbuffN1(6)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(4) & "." & strhexbuffN3(5) & "." & strhexbuffN3(6)

                    If strhexbuffN1(4) = CharNVM(1) And strhexbuffN1(5) = CharNVM(2) And strhexbuffN1(6) = CharNVM(3) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 1|F|CHECK NVM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If strhexbuffN3(4) = CharNVM(1) And strhexbuffN3(5) = CharNVM(2) And strhexbuffN3(6) = CharNVM(3) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 3|F|CHECK NVM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                    '===== CHECK NVM NEST 1, NEST 4 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    ThreadCheckNVMN1.Start()
                    ThreadCheckNVMN1.Join()

                    ThreadCheckNVMN4.Start()
                    ThreadCheckNVMN4.Join()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(4) & "." & strhexbuffN1(5) & "." & strhexbuffN1(6)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(4) & "." & strhexbuffN4(5) & "." & strhexbuffN4(6)

                    If strhexbuffN1(4) = CharNVM(1) And strhexbuffN1(5) = CharNVM(2) And strhexbuffN1(6) = CharNVM(3) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 1|F|CHECK NVM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If strhexbuffN4(4) = CharNVM(1) And strhexbuffN4(5) = CharNVM(2) And strhexbuffN4(6) = CharNVM(3) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 4|F|CHECK NVM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                    '===== CHECK NVM NEST 2, NEST 3 ====='

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    ThreadCheckNVMN2.Start()
                    ThreadCheckNVMN2.Join()

                    ThreadCheckNVMN3.Start()
                    ThreadCheckNVMN3.Join()

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(4) & "." & strhexbuffN2(5) & "." & strhexbuffN2(6)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(4) & "." & strhexbuffN3(5) & "." & strhexbuffN3(6)

                    If strhexbuffN2(4) = CharNVM(1) And strhexbuffN2(5) = CharNVM(2) And strhexbuffN2(6) = CharNVM(3) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 2|F|CHECK NVM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If strhexbuffN3(4) = CharNVM(1) And strhexbuffN3(5) = CharNVM(2) And strhexbuffN3(6) = CharNVM(3) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 3|F|CHECK NVM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                    '===== CHECK NVM NEST 2, NEST 4 ====='

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(4) & "." & strhexbuffN2(5) & "." & strhexbuffN2(6)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(4) & "." & strhexbuffN4(5) & "." & strhexbuffN4(6)

                    If strhexbuffN2(4) = CharNVM(1) And strhexbuffN2(5) = CharNVM(2) And strhexbuffN2(6) = CharNVM(3) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 2|F|CHECK NVM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If strhexbuffN4(4) = CharNVM(1) And strhexbuffN4(5) = CharNVM(2) And strhexbuffN4(6) = CharNVM(3) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 4|F|CHECK NVM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                    '===== CHECK NVM NEST 3, NEST 4 ====='

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    ThreadCheckNVMN3.Start()
                    ThreadCheckNVMN3.Join()

                    ThreadCheckNVMN4.Start()
                    ThreadCheckNVMN4.Join()

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(4) & "." & strhexbuffN3(5) & "." & strhexbuffN3(6)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(4) & "." & strhexbuffN4(5) & "." & strhexbuffN4(6)

                    If strhexbuffN3(4) = CharNVM(1) And strhexbuffN3(5) = CharNVM(2) And strhexbuffN3(6) = CharNVM(3) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 3|F|CHECK NVM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If strhexbuffN4(4) = CharNVM(1) And strhexbuffN4(5) = CharNVM(2) And strhexbuffN4(6) = CharNVM(3) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 4|F|CHECK NVM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                    '===== CHECK NVM NEST 1 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    CheckNVMN1()

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN1(4) & "." & strhexbuffN1(5) & "." & strhexbuffN1(6)

                    If strhexbuffN1(4) = CharNVM(1) And strhexbuffN1(5) = CharNVM(2) And strhexbuffN1(6) = CharNVM(3) Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 1|F|CHECK NVM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                    '===== CHECK NVM NEST 2 ====='

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    CheckNVMN2()

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN2(4) & "." & strhexbuffN2(5) & "." & strhexbuffN2(6)

                    If strhexbuffN2(4) = CharNVM(1) And strhexbuffN2(5) = CharNVM(2) And strhexbuffN2(6) = CharNVM(3) Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 2|F|CHECK NVM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                    '===== CHECK NVM NEST 3 ====='

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    CheckNVMN3()

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN3(4) & "." & strhexbuffN3(5) & "." & strhexbuffN3(6)

                    If strhexbuffN3(4) = CharNVM(1) And strhexbuffN3(5) = CharNVM(2) And strhexbuffN3(6) = CharNVM(3) Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 3|F|CHECK NVM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                    '===== CHECK NVM NEST 4 ====='

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Ver. N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = CharNVM(1) & "." & CharNVM(2) & "." & CharNVM(3)

                    CheckNVMN4()

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = strhexbuffN4(4) & "." & strhexbuffN4(5) & "." & strhexbuffN4(6)

                    If strhexbuffN4(4) = CharNVM(1) And strhexbuffN4(5) = CharNVM(2) And strhexbuffN4(6) = CharNVM(3) Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM NEST 4|F|CHECK NVM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                    StepTest = 123

                End If

            Case 5

                ProgressBarProcess.Value = StepTest * 1.75
                StepProcess = StepTest

                ValidateNVMN1 = False
                ValidateNVMN2 = False
                ValidateNVMN3 = False
                ValidateNVMN4 = False

                If ProcessN1 = True And ProcessN2 = True And ProcessN3 = True And ProcessN4 = True Then

                    '===== CHECK NVM DATA ALL NEST ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    ThreadCheckNVMDataN1.Start()
                    ThreadCheckNVMDataN1.Join()

                    ThreadCheckNVMDataN2.Start()
                    ThreadCheckNVMDataN2.Join()

                    ThreadCheckNVMDataN3.Start()
                    ThreadCheckNVMDataN3.Join()

                    ThreadCheckNVMDataN4.Start()
                    ThreadCheckNVMDataN4.Join()

                    If ValidateNVMN1 = True Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 1|F|CHECK NVM DATA NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If ValidateNVMN2 = True Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 2|F|CHECK NVM DATA NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If ValidateNVMN3 = True Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 3|F|CHECK NVM DATA NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If ValidateNVMN4 = True Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 4|F|CHECK NVM DATA NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                    '===== CHECK NVM DATA NEST 1, NEST 2, NEST 3 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    ThreadCheckNVMDataN1.Start()
                    ThreadCheckNVMDataN1.Join()

                    ThreadCheckNVMDataN2.Start()
                    ThreadCheckNVMDataN2.Join()

                    ThreadCheckNVMDataN3.Start()
                    ThreadCheckNVMDataN3.Join()

                    If ValidateNVMN1 = True Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 1|F|CHECK NVM DATA NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If ValidateNVMN2 = True Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 2|F|CHECK NVM DATA NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If ValidateNVMN3 = True Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 3|F|CHECK NVM DATA NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                    '===== CHECK NVM DATA NEST 1, NEST 2, NEST 4 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    ThreadCheckNVMDataN1.Start()
                    ThreadCheckNVMDataN1.Join()

                    ThreadCheckNVMDataN2.Start()
                    ThreadCheckNVMDataN2.Join()

                    ThreadCheckNVMDataN4.Start()
                    ThreadCheckNVMDataN4.Join()

                    If ValidateNVMN1 = True Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 1|F|CHECK NVM DATA NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If ValidateNVMN2 = True Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 2|F|CHECK NVM DATA NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If ValidateNVMN4 = True Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 4|F|CHECK NVM DATA NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                    '===== CHECK NVM DATA NEST 1, NEST 3, NEST 4 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    ThreadCheckNVMDataN1.Start()
                    ThreadCheckNVMDataN1.Join()

                    ThreadCheckNVMDataN3.Start()
                    ThreadCheckNVMDataN3.Join()

                    ThreadCheckNVMDataN4.Start()
                    ThreadCheckNVMDataN4.Join()

                    If ValidateNVMN1 = True Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 1|F|CHECK NVM DATA NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If ValidateNVMN3 = True Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 3|F|CHECK NVM DATA NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If ValidateNVMN4 = True Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 4|F|CHECK NVM DATA NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = True Then

                    '===== CHECK NVM DATA NEST 2, NEST 3, NEST 4 ====='

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    ThreadCheckNVMDataN2.Start()
                    ThreadCheckNVMDataN2.Join()

                    ThreadCheckNVMDataN3.Start()
                    ThreadCheckNVMDataN3.Join()

                    ThreadCheckNVMDataN4.Start()
                    ThreadCheckNVMDataN4.Join()

                    If ValidateNVMN2 = True Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 2|F|CHECK NVM DATA NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If ValidateNVMN3 = True Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 3|F|CHECK NVM DATA NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If ValidateNVMN4 = True Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 4|F|CHECK NVM DATA NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                    '===== CHECK NVM DATA NEST 1, NEST 2 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    ThreadCheckNVMDataN1.Start()
                    ThreadCheckNVMDataN1.Join()

                    ThreadCheckNVMDataN2.Start()
                    ThreadCheckNVMDataN2.Join()

                    If ValidateNVMN1 = True Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 1|F|CHECK NVM DATA NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If ValidateNVMN2 = True Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 2|F|CHECK NVM DATA NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                    '===== CHECK NVM DATA NEST 1, NEST 3 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    ThreadCheckNVMDataN1.Start()
                    ThreadCheckNVMDataN1.Join()

                    ThreadCheckNVMDataN3.Start()
                    ThreadCheckNVMDataN3.Join()

                    If ValidateNVMN1 = True Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 1|F|CHECK NVM DATA NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If ValidateNVMN3 = True Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 3|F|CHECK NVM DATA NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                    '===== CHECK NVM DATA NEST 1, NEST 4 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    ThreadCheckNVMDataN1.Start()
                    ThreadCheckNVMDataN1.Join()

                    ThreadCheckNVMDataN4.Start()
                    ThreadCheckNVMDataN4.Join()

                    If ValidateNVMN1 = True Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 1|F|CHECK NVM DATA NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If ValidateNVMN4 = True Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 4|F|CHECK NVM DATA NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = True And ProcessN4 = False Then

                    '===== CHECK NVM DATA NEST 2, NEST 3 ====='

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    ThreadCheckNVMDataN2.Start()
                    ThreadCheckNVMDataN2.Join()

                    ThreadCheckNVMDataN3.Start()
                    ThreadCheckNVMDataN3.Join()

                    If ValidateNVMN2 = True Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 2|F|CHECK NVM DATA NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If ValidateNVMN3 = True Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 3|F|CHECK NVM DATA NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = True Then

                    '===== CHECK NVM DATA NEST 2, NEST 4 ====='

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    ThreadCheckNVMDataN2.Start()
                    ThreadCheckNVMDataN2.Join()

                    ThreadCheckNVMDataN4.Start()
                    ThreadCheckNVMDataN4.Join()

                    If ValidateNVMN2 = True Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 2|F|CHECK NVM DATA NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If ValidateNVMN4 = True Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 4|F|CHECK NVM DATA NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True And ProcessN4 = True Then

                    '===== CHECK NVM DATA NEST 3, NEST 4 ====='

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    ThreadCheckNVMDataN3.Start()
                    ThreadCheckNVMDataN3.Join()

                    ThreadCheckNVMDataN4.Start()
                    ThreadCheckNVMDataN4.Join()

                    If ValidateNVMN3 = True Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 3|F|CHECK NVM DATA NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If ValidateNVMN4 = True Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 4|F|CHECK NVM DATA NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = True And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                    '===== CHECK NVM DATA NEST 1 ====='

                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N1"
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    CheckNVMDataN1()

                    If ValidateNVMN1 = True Then

                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN1 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 1|F|CHECK NVM DATA NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                        Nest1NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = True And ProcessN3 = False And ProcessN4 = False Then

                    '===== CHECK NVM DATA NEST 2 ====='

                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N2"
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    CheckNVMDataN2()

                    If ValidateNVMN2 = True Then

                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN2 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 2|F|CHECK NVM DATA NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                        Nest2NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = True And ProcessN4 = False Then

                    '===== CHECK NVM DATA NEST 3 ====='

                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N3"
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    CheckNVMDataN3()

                    If ValidateNVMN3 = True Then

                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN3 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 3|F|CHECK NVM DATA NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                        Nest3NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = True Then

                    '===== CHECK NVM DATA NEST 4 ====='

                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = "Check NVM Data N4"
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Style.BackColor = Color.LightGray
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Style.BackColor = Color.LightGray

                    CheckNVMDataN4()

                    If ValidateNVMN4 = True Then

                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        ProcessN4 = True

                    Else

                        NGType = "|*|FUN|*|1|1|CHECK NVM DATA NEST 4|F|CHECK NVM DATA NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                        Nest4NG()

                    End If

                    If ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then
                        StepTest = 123
                    Else
                        StepTest += 1
                    End If

                ElseIf ProcessN1 = False And ProcessN2 = False And ProcessN3 = False And ProcessN4 = False Then

                    StepTest = 123

                End If

            Case 6
                ProgressBarProcess.Value = StepTest * 1.75
                StepProcess = StepTest

                '===== PREPARE IGNITION + ALL LED ON ALL NEST ====='

                PSU13V()
                Delay_Ms(100)

                BootmodeN1Off()
                Delay_Ms(100)

                BootmodeN2Off()
                Delay_Ms(100)

                BootmodeN3Off()
                Delay_Ms(100)

                BootmodeN4Off()
                Delay_Ms(100)

                If ProcessN1 = True Then
                    IgnitionN1On()
                    Delay_Ms(200)
                    AllLEDN1On()
                    Delay_Ms(100)

                    Feasa.RunningFeasa1()
                    Delay_Ms(200)
                End If

                If ProcessN2 = True Then
                    IgnitionN2On()
                    Delay_Ms(200)
                    AllLEDN2On()
                    Delay_Ms(100)

                    Feasa.RunningFeasa2()
                    Delay_Ms(200)
                End If

                If ProcessN3 = True Then
                    IgnitionN3On()
                    Delay_Ms(200)
                    AllLEDN3On()
                    Delay_Ms(100)

                    Feasa.RunningFeasa3()
                    Delay_Ms(200)
                End If

                If ProcessN4 = True Then
                    IgnitionN4On()
                    Delay_Ms(200)
                    AllLEDN4On()
                    Delay_Ms(100)

                    Feasa.RunningFeasa4()
                    Delay_Ms(200)
                End If

                StepTest = 7

            Case 7
                StepProcess = StepTest

                '====== NEST 1 ======'
                If ProcessN1 = True Then
                    DataGridViewProcessNest1.FirstDisplayedScrollingRowIndex = 7
                    ProgressBarProcess.Value = StepTest * 1.75

                    '====== TURN LEFT ======'
                    CheckPoint = ItemCheck(2)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 2).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest1.Rows.Item(StepTest - 2).Cells(2).Value = "H: " & My.Settings.Feasa1HueMin2 & " & " & "I: " & My.Settings.Feasa1IntenMin2
                    DataGridViewProcessNest1.Rows.Item(StepTest - 2).Cells(3).Value = "H: " & My.Settings.Feasa1HueMax2 & " & " & "I: " & My.Settings.Feasa1IntenMax2
                    DataGridViewProcessNest1.Rows.Item(StepTest - 2).Cells(4).Value = "H: " & Feasa.Value1_Hue2 & " & " & "I: " & Feasa.Value1_Inten2
                    If Feasa.Condition1_Hue2 = True And Feasa.Condition1_Inten2 = True Then
                        DataGridViewProcessNest1.Rows.Item(StepTest - 2).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 2).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK TURN LEFT NEST 1|F|CHECK TURN LEFT NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 2).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest - 2).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                    End If

                    '====== SMART ======'
                    CheckPoint = ItemCheck(3)
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(2).Value = "H: " & My.Settings.Feasa1HueMin3 & " & " & "I: " & My.Settings.Feasa1IntenMin3
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(3).Value = "H: " & My.Settings.Feasa1HueMax3 & " & " & "I: " & My.Settings.Feasa1IntenMax3
                    DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(4).Value = "H: " & Feasa.Value1_Hue3 & " & " & "I: " & Feasa.Value1_Inten3
                    If ComboBoxModel.Text = "CG" Or ComboBoxModel.Text = "DG" Or ComboBoxModel.Text = "JG" Or ComboBoxModel.Text = "KG" Or
                        ComboBoxModel.Text = "CH" Or ComboBoxModel.Text = "DH" Or ComboBoxModel.Text = "JH" Or ComboBoxModel.Text = "KH" Then
                        If Feasa.Condition1_Hue3 = False And Feasa.Condition1_Inten3 = False Then
                            DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                            DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK SMART NEST 1|F|CHECK SMART NEST 1|||"
                            DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                            DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                            ProcessN1 = False
                        End If
                    Else
                        If Feasa.Condition1_Hue3 = True And Feasa.Condition1_Inten3 = True Then
                            DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                            DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK SMART NEST 1|F|CHECK SMART NEST 1|||"
                            DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                            DataGridViewProcessNest1.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                            ProcessN1 = False
                        End If
                    End If

                    '====== HIGH BEAM ======'
                    CheckPoint = ItemCheck(4)
                    DataGridViewProcessNest1.Rows.Item(StepTest).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest1.Rows.Item(StepTest).Cells(2).Value = "H: " & My.Settings.Feasa1HueMin4 & " & " & "I: " & My.Settings.Feasa1IntenMin4
                    DataGridViewProcessNest1.Rows.Item(StepTest).Cells(3).Value = "H: " & My.Settings.Feasa1HueMax4 & " & " & "I: " & My.Settings.Feasa1IntenMax4
                    DataGridViewProcessNest1.Rows.Item(StepTest).Cells(4).Value = "H: " & Feasa.Value1_Hue4 & " & " & "I: " & Feasa.Value1_Inten4
                    If Feasa.Condition1_Hue4 = True And Feasa.Condition1_Inten4 = True Then
                        DataGridViewProcessNest1.Rows.Item(StepTest).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK HIGH BEAM NEST 1|F|CHECK HIGH BEAM NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                    End If

                    '====== ENGINE ======'
                    CheckPoint = ItemCheck(5)
                    DataGridViewProcessNest1.Rows.Item(StepTest + 1).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest1.Rows.Item(StepTest + 1).Cells(2).Value = "H: " & My.Settings.Feasa1HueMin5 & " & " & "I: " & My.Settings.Feasa1IntenMin5
                    DataGridViewProcessNest1.Rows.Item(StepTest + 1).Cells(3).Value = "H: " & My.Settings.Feasa1HueMax5 & " & " & "I: " & My.Settings.Feasa1IntenMax5
                    DataGridViewProcessNest1.Rows.Item(StepTest + 1).Cells(4).Value = "H: " & Feasa.Value1_Hue5 & " & " & "I: " & Feasa.Value1_Inten5
                    If Feasa.Condition1_Hue5 = True And Feasa.Condition1_Inten5 = True Then
                        DataGridViewProcessNest1.Rows.Item(StepTest + 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest + 1).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK ENGINE NEST 1|F|CHECK ENGINE NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest + 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest + 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                    End If

                    '====== TW ======'
                    CheckPoint = ItemCheck(6)
                    DataGridViewProcessNest1.Rows.Item(StepTest + 2).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest1.Rows.Item(StepTest + 2).Cells(2).Value = "H: " & My.Settings.Feasa1HueMin6 & " & " & "I: " & My.Settings.Feasa1IntenMin6
                    DataGridViewProcessNest1.Rows.Item(StepTest + 2).Cells(3).Value = "H: " & My.Settings.Feasa1HueMax6 & " & " & "I: " & My.Settings.Feasa1IntenMax6
                    DataGridViewProcessNest1.Rows.Item(StepTest + 2).Cells(4).Value = "H: " & Feasa.Value1_Hue6 & " & " & "I: " & Feasa.Value1_Inten6
                    If Feasa.Condition1_Hue6 = True And Feasa.Condition1_Inten6 = True Then
                        DataGridViewProcessNest1.Rows.Item(StepTest + 2).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest + 2).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK TW NEST 1|F|CHECK TW NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest + 2).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest + 2).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                    End If

                    '====== ISS ======'
                    CheckPoint = ItemCheck(7)
                    DataGridViewProcessNest1.Rows.Item(StepTest + 3).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest1.Rows.Item(StepTest + 3).Cells(2).Value = "H: " & My.Settings.Feasa1HueMin7 & " & " & "I: " & My.Settings.Feasa1IntenMin7
                    DataGridViewProcessNest1.Rows.Item(StepTest + 3).Cells(3).Value = "H: " & My.Settings.Feasa1HueMax7 & " & " & "I: " & My.Settings.Feasa1IntenMax7
                    DataGridViewProcessNest1.Rows.Item(StepTest + 3).Cells(4).Value = "H: " & Feasa.Value1_Hue7 & " & " & "I: " & Feasa.Value1_Inten7
                    If ComboBoxModel.Text = "DG" Or ComboBoxModel.Text = "KG" Or
                        ComboBoxModel.Text = "DH" Or ComboBoxModel.Text = "KH" Then
                        If Feasa.Condition1_Hue7 = False And Feasa.Condition1_Inten7 = False Then
                            DataGridViewProcessNest1.Rows.Item(StepTest + 3).Cells(5).Value = "OK"
                            DataGridViewProcessNest1.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ISS NEST 1|F|CHECK ISS NEST 1|||"
                            DataGridViewProcessNest1.Rows.Item(StepTest + 3).Cells(5).Value = "NG"
                            DataGridViewProcessNest1.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.Red
                            ProcessN1 = False
                        End If
                    Else
                        If Feasa.Condition1_Hue7 = True And Feasa.Condition1_Inten7 = True Then
                            DataGridViewProcessNest1.Rows.Item(StepTest + 3).Cells(5).Value = "OK"
                            DataGridViewProcessNest1.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ISS NEST 1|F|CHECK ISS NEST 1|||"
                            DataGridViewProcessNest1.Rows.Item(StepTest + 3).Cells(5).Value = "NG"
                            DataGridViewProcessNest1.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.Red
                            ProcessN1 = False
                        End If
                    End If

                    '====== BATTERY FAIL ======'
                    CheckPoint = ItemCheck(8)
                    DataGridViewProcessNest1.Rows.Item(StepTest + 4).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest1.Rows.Item(StepTest + 4).Cells(2).Value = "H: " & My.Settings.Feasa1HueMin8 & " & " & "I: " & My.Settings.Feasa1IntenMin8
                    DataGridViewProcessNest1.Rows.Item(StepTest + 4).Cells(3).Value = "H: " & My.Settings.Feasa1HueMax8 & " & " & "I: " & My.Settings.Feasa1IntenMax8
                    DataGridViewProcessNest1.Rows.Item(StepTest + 4).Cells(4).Value = "H: " & Feasa.Value1_Hue8 & " & " & "I: " & Feasa.Value1_Inten8
                    If Feasa.Condition1_Hue8 = True And Feasa.Condition1_Inten8 = True Then
                        DataGridViewProcessNest1.Rows.Item(StepTest + 4).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest + 4).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK BATTERY FAIL NEST 1|F|CHECK BATTERY FAIL NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest + 4).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest + 4).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                    End If

                    '====== TURN RIGHT ======'
                    CheckPoint = ItemCheck(9)
                    DataGridViewProcessNest1.Rows.Item(StepTest + 5).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest1.Rows.Item(StepTest + 5).Cells(2).Value = "H: " & My.Settings.Feasa1HueMin9 & " & " & "I: " & My.Settings.Feasa1IntenMin9
                    DataGridViewProcessNest1.Rows.Item(StepTest + 5).Cells(3).Value = "H: " & My.Settings.Feasa1HueMax9 & " & " & "I: " & My.Settings.Feasa1IntenMax9
                    DataGridViewProcessNest1.Rows.Item(StepTest + 5).Cells(4).Value = "H: " & Feasa.Value1_Hue9 & " & " & "I: " & Feasa.Value1_Inten9
                    If Feasa.Condition1_Hue9 = True And Feasa.Condition1_Inten9 = True Then
                        DataGridViewProcessNest1.Rows.Item(StepTest + 5).Cells(5).Value = "OK"
                        DataGridViewProcessNest1.Rows.Item(StepTest + 5).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK TURN RIGHT NEST 1|F|CHECK TURN RIGHT NEST 1|||"
                        DataGridViewProcessNest1.Rows.Item(StepTest + 5).Cells(5).Value = "NG"
                        DataGridViewProcessNest1.Rows.Item(StepTest + 5).Cells(5).Style.BackColor = Color.Red
                        ProcessN1 = False
                    End If

                    '====== ALARM ======'
                    CheckPoint = ItemCheck(10)
                    DataGridViewProcessNest1.Rows.Item(StepTest + 6).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest1.Rows.Item(StepTest + 6).Cells(2).Value = "H: " & My.Settings.Feasa1HueMin10 & " & " & "I: " & My.Settings.Feasa1IntenMin10
                    DataGridViewProcessNest1.Rows.Item(StepTest + 6).Cells(3).Value = "H: " & My.Settings.Feasa1HueMax10 & " & " & "I: " & My.Settings.Feasa1IntenMax10
                    DataGridViewProcessNest1.Rows.Item(StepTest + 6).Cells(4).Value = "H: " & Feasa.Value1_Hue10 & " & " & "I: " & Feasa.Value1_Inten10
                    If ComboBoxModel.Text = "DG" Or ComboBoxModel.Text = "KG" Or
                        ComboBoxModel.Text = "DH" Or ComboBoxModel.Text = "KH" Then
                        If Feasa.Condition1_Hue10 = False And Feasa.Condition1_Inten10 = False Then
                            DataGridViewProcessNest1.Rows.Item(StepTest + 6).Cells(5).Value = "OK"
                            DataGridViewProcessNest1.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ALARM NEST 1|F|CHECK ALARM NEST 1|||"
                            DataGridViewProcessNest1.Rows.Item(StepTest + 6).Cells(5).Value = "NG"
                            DataGridViewProcessNest1.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.Red
                            ProcessN1 = False
                        End If
                    Else
                        If Feasa.Condition1_Hue10 = True And Feasa.Condition1_Inten10 = True Then
                            DataGridViewProcessNest1.Rows.Item(StepTest + 6).Cells(5).Value = "OK"
                            DataGridViewProcessNest1.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ALARM NEST 1|F|CHECK ALARM NEST 1|||"
                            DataGridViewProcessNest1.Rows.Item(StepTest + 6).Cells(5).Value = "NG"
                            DataGridViewProcessNest1.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.Red
                            ProcessN1 = False
                        End If
                    End If

                    If ProcessN1 = False Then
                        Nest1NG()
                    End If
                End If

                '====== NEST 2 ======'
                If ProcessN2 = True Then
                    DataGridViewProcessNest2.FirstDisplayedScrollingRowIndex = 7
                    ProgressBarProcess.Value = StepTest * 1.75

                    '====== TURN LEFT ======'
                    CheckPoint = ItemCheck(2)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 2).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest2.Rows.Item(StepTest - 2).Cells(2).Value = "H: " & My.Settings.Feasa2HueMin2 & " & " & "I: " & My.Settings.Feasa2IntenMin2
                    DataGridViewProcessNest2.Rows.Item(StepTest - 2).Cells(3).Value = "H: " & My.Settings.Feasa2HueMax2 & " & " & "I: " & My.Settings.Feasa2IntenMax2
                    DataGridViewProcessNest2.Rows.Item(StepTest - 2).Cells(4).Value = "H: " & Feasa.Value2_Hue2 & " & " & "I: " & Feasa.Value2_Inten2
                    If Feasa.Condition2_Hue2 = True And Feasa.Condition2_Inten2 = True Then
                        DataGridViewProcessNest2.Rows.Item(StepTest - 2).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 2).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK TURN LEFT NEST 2|F|CHECK TURN LEFT NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 2).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest - 2).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                    End If

                    '====== SMART ======'
                    CheckPoint = ItemCheck(3)
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(2).Value = "H: " & My.Settings.Feasa2HueMin3 & " & " & "I: " & My.Settings.Feasa2IntenMin3
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(3).Value = "H: " & My.Settings.Feasa2HueMax3 & " & " & "I: " & My.Settings.Feasa2IntenMax3
                    DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(4).Value = "H: " & Feasa.Value2_Hue3 & " & " & "I: " & Feasa.Value2_Inten3
                    If ComboBoxModel.Text = "CG" Or ComboBoxModel.Text = "DG" Or ComboBoxModel.Text = "JG" Or ComboBoxModel.Text = "KG" Or
                        ComboBoxModel.Text = "CH" Or ComboBoxModel.Text = "DH" Or ComboBoxModel.Text = "JH" Or ComboBoxModel.Text = "KH" Then
                        If Feasa.Condition2_Hue3 = False And Feasa.Condition2_Inten3 = False Then
                            DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                            DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK SMART NEST 2|F|CHECK SMART NEST 2|||"
                            DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                            DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                            ProcessN2 = False
                        End If
                    Else
                        If Feasa.Condition2_Hue3 = True And Feasa.Condition2_Inten3 = True Then
                            DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                            DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK SMART NEST 2|F|CHECK SMART NEST 2|||"
                            DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                            DataGridViewProcessNest2.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                            ProcessN2 = False
                        End If
                    End If

                    '====== HIGH BEAM ======'
                    CheckPoint = ItemCheck(4)
                    DataGridViewProcessNest2.Rows.Item(StepTest).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest2.Rows.Item(StepTest).Cells(2).Value = "H: " & My.Settings.Feasa2HueMin4 & " & " & "I: " & My.Settings.Feasa2IntenMin4
                    DataGridViewProcessNest2.Rows.Item(StepTest).Cells(3).Value = "H: " & My.Settings.Feasa2HueMax4 & " & " & "I: " & My.Settings.Feasa2IntenMax4
                    DataGridViewProcessNest2.Rows.Item(StepTest).Cells(4).Value = "H: " & Feasa.Value2_Hue4 & " & " & "I: " & Feasa.Value2_Inten4
                    If Feasa.Condition2_Hue4 = True And Feasa.Condition2_Inten4 = True Then
                        DataGridViewProcessNest2.Rows.Item(StepTest).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK HIGH BEAM NEST 2|F|CHECK HIGH BEAM NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                    End If

                    '====== ENGINE ======'
                    CheckPoint = ItemCheck(5)
                    DataGridViewProcessNest2.Rows.Item(StepTest + 1).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest2.Rows.Item(StepTest + 1).Cells(2).Value = "H: " & My.Settings.Feasa2HueMin6 & " & " & "I: " & My.Settings.Feasa2IntenMin6
                    DataGridViewProcessNest2.Rows.Item(StepTest + 1).Cells(3).Value = "H: " & My.Settings.Feasa2HueMax6 & " & " & "I: " & My.Settings.Feasa2IntenMax6
                    DataGridViewProcessNest2.Rows.Item(StepTest + 1).Cells(4).Value = "H: " & Feasa.Value2_Hue6 & " & " & "I: " & Feasa.Value2_Inten6
                    If Feasa.Condition2_Hue6 = True And Feasa.Condition2_Inten6 = True Then
                        DataGridViewProcessNest2.Rows.Item(StepTest + 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest + 1).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK ENGINE NEST 2|F|CHECK ENGINE NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest + 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest + 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                    End If

                    '====== TW ======'
                    CheckPoint = ItemCheck(6)
                    DataGridViewProcessNest2.Rows.Item(StepTest + 2).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest2.Rows.Item(StepTest + 2).Cells(2).Value = "H: " & My.Settings.Feasa2HueMin7 & " & " & "I: " & My.Settings.Feasa2IntenMin7
                    DataGridViewProcessNest2.Rows.Item(StepTest + 2).Cells(3).Value = "H: " & My.Settings.Feasa2HueMax7 & " & " & "I: " & My.Settings.Feasa2IntenMax7
                    DataGridViewProcessNest2.Rows.Item(StepTest + 2).Cells(4).Value = "H: " & Feasa.Value2_Hue7 & " & " & "I: " & Feasa.Value2_Inten7
                    If Feasa.Condition2_Hue7 = True And Feasa.Condition2_Inten7 = True Then
                        DataGridViewProcessNest2.Rows.Item(StepTest + 2).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest + 2).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK TW NEST 2|F|CHECK TW NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest + 2).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest + 2).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                    End If

                    '====== ISS ======'
                    CheckPoint = ItemCheck(7)
                    DataGridViewProcessNest2.Rows.Item(StepTest + 3).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest2.Rows.Item(StepTest + 3).Cells(2).Value = "H: " & My.Settings.Feasa2HueMin8 & " & " & "I: " & My.Settings.Feasa2IntenMin8
                    DataGridViewProcessNest2.Rows.Item(StepTest + 3).Cells(3).Value = "H: " & My.Settings.Feasa2HueMax8 & " & " & "I: " & My.Settings.Feasa2IntenMax8
                    DataGridViewProcessNest2.Rows.Item(StepTest + 3).Cells(4).Value = "H: " & Feasa.Value2_Hue8 & " & " & "I: " & Feasa.Value2_Inten8
                    If ComboBoxModel.Text = "DG" Or ComboBoxModel.Text = "KG" Or
                        ComboBoxModel.Text = "DH" Or ComboBoxModel.Text = "KH" Then
                        If Feasa.Condition2_Hue8 = False And Feasa.Condition2_Inten8 = False Then
                            DataGridViewProcessNest2.Rows.Item(StepTest + 3).Cells(5).Value = "OK"
                            DataGridViewProcessNest2.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ISS NEST 2|F|CHECK ISS NEST 2|||"
                            DataGridViewProcessNest2.Rows.Item(StepTest + 3).Cells(5).Value = "NG"
                            DataGridViewProcessNest2.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.Red
                            ProcessN2 = False
                        End If
                    Else
                        If Feasa.Condition2_Hue8 = True And Feasa.Condition2_Inten8 = True Then
                            DataGridViewProcessNest2.Rows.Item(StepTest + 3).Cells(5).Value = "OK"
                            DataGridViewProcessNest2.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ISS NEST 2|F|CHECK ISS NEST 2|||"
                            DataGridViewProcessNest2.Rows.Item(StepTest + 3).Cells(5).Value = "NG"
                            DataGridViewProcessNest2.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.Red
                            ProcessN2 = False
                        End If
                    End If

                    '====== BATTERY FAIL ======'
                    CheckPoint = ItemCheck(8)
                    DataGridViewProcessNest2.Rows.Item(StepTest + 4).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest2.Rows.Item(StepTest + 4).Cells(2).Value = "H: " & My.Settings.Feasa2HueMin9 & " & " & "I: " & My.Settings.Feasa2IntenMin9
                    DataGridViewProcessNest2.Rows.Item(StepTest + 4).Cells(3).Value = "H: " & My.Settings.Feasa2HueMax9 & " & " & "I: " & My.Settings.Feasa2IntenMax9
                    DataGridViewProcessNest2.Rows.Item(StepTest + 4).Cells(4).Value = "H: " & Feasa.Value2_Hue9 & " & " & "I: " & Feasa.Value2_Inten9
                    If Feasa.Condition2_Hue9 = True And Feasa.Condition2_Inten9 = True Then
                        DataGridViewProcessNest2.Rows.Item(StepTest + 4).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest + 4).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK BATTERY FAIL NEST 2|F|CHECK BATTERY FAIL NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest + 4).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest + 4).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                    End If

                    '====== TURN RIGHT ======'
                    CheckPoint = ItemCheck(9)
                    DataGridViewProcessNest2.Rows.Item(StepTest + 5).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest2.Rows.Item(StepTest + 5).Cells(2).Value = "H: " & My.Settings.Feasa2HueMin10 & " & " & "I: " & My.Settings.Feasa2IntenMin10
                    DataGridViewProcessNest2.Rows.Item(StepTest + 5).Cells(3).Value = "H: " & My.Settings.Feasa2HueMax10 & " & " & "I: " & My.Settings.Feasa2IntenMax10
                    DataGridViewProcessNest2.Rows.Item(StepTest + 5).Cells(4).Value = "H: " & Feasa.Value2_Hue10 & " & " & "I: " & Feasa.Value2_Inten10
                    If Feasa.Condition2_Hue10 = True And Feasa.Condition2_Inten10 = True Then
                        DataGridViewProcessNest2.Rows.Item(StepTest + 5).Cells(5).Value = "OK"
                        DataGridViewProcessNest2.Rows.Item(StepTest + 5).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK TURN RIGHT NEST 2|F|CHECK TURN RIGHT NEST 2|||"
                        DataGridViewProcessNest2.Rows.Item(StepTest + 5).Cells(5).Value = "NG"
                        DataGridViewProcessNest2.Rows.Item(StepTest + 5).Cells(5).Style.BackColor = Color.Red
                        ProcessN2 = False
                    End If

                    '====== ALARM ======'
                    CheckPoint = ItemCheck(10)
                    DataGridViewProcessNest2.Rows.Item(StepTest + 6).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest2.Rows.Item(StepTest + 6).Cells(2).Value = "H: " & My.Settings.Feasa2HueMin11 & " & " & "I: " & My.Settings.Feasa2IntenMin11
                    DataGridViewProcessNest2.Rows.Item(StepTest + 6).Cells(3).Value = "H: " & My.Settings.Feasa2HueMax11 & " & " & "I: " & My.Settings.Feasa2IntenMax11
                    DataGridViewProcessNest2.Rows.Item(StepTest + 6).Cells(4).Value = "H: " & Feasa.Value2_Hue11 & " & " & "I: " & Feasa.Value2_Inten11
                    If ComboBoxModel.Text = "DG" Or ComboBoxModel.Text = "KG" Or
                        ComboBoxModel.Text = "DH" Or ComboBoxModel.Text = "KH" Then
                        If Feasa.Condition2_Hue11 = False And Feasa.Condition2_Inten11 = False Then
                            DataGridViewProcessNest2.Rows.Item(StepTest + 6).Cells(5).Value = "OK"
                            DataGridViewProcessNest2.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ALARM NEST 2|F|CHECK ALARM NEST 2|||"
                            DataGridViewProcessNest2.Rows.Item(StepTest + 6).Cells(5).Value = "NG"
                            DataGridViewProcessNest2.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.Red
                            ProcessN2 = False
                        End If
                    Else
                        If Feasa.Condition2_Hue11 = True And Feasa.Condition2_Inten11 = True Then
                            DataGridViewProcessNest2.Rows.Item(StepTest + 6).Cells(5).Value = "OK"
                            DataGridViewProcessNest2.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ALARM NEST 2|F|CHECK ALARM NEST 2|||"
                            DataGridViewProcessNest2.Rows.Item(StepTest + 6).Cells(5).Value = "NG"
                            DataGridViewProcessNest2.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.Red
                            ProcessN2 = False
                        End If
                    End If

                    If ProcessN2 = False Then
                        Nest2NG()
                    End If
                End If

                '====== NEST 3 ======'
                If ProcessN3 = True Then
                    DataGridViewProcessNest3.FirstDisplayedScrollingRowIndex = 7
                    ProgressBarProcess.Value = StepTest * 1.75

                    '====== TURN LEFT ======'
                    CheckPoint = ItemCheck(2)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 2).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest3.Rows.Item(StepTest - 2).Cells(2).Value = "H: " & My.Settings.Feasa3HueMin2 & " & " & "I: " & My.Settings.Feasa3IntenMin2
                    DataGridViewProcessNest3.Rows.Item(StepTest - 2).Cells(3).Value = "H: " & My.Settings.Feasa3HueMax2 & " & " & "I: " & My.Settings.Feasa3IntenMax2
                    DataGridViewProcessNest3.Rows.Item(StepTest - 2).Cells(4).Value = "H: " & Feasa.Value3_Hue2 & " & " & "I: " & Feasa.Value3_Inten2
                    If Feasa.Condition3_Hue2 = True And Feasa.Condition3_Inten2 = True Then
                        DataGridViewProcessNest3.Rows.Item(StepTest - 2).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 2).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK TURN LEFT NEST 3|F|CHECK TURN LEFT NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 2).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest - 2).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                    End If

                    '====== SMART ======'
                    CheckPoint = ItemCheck(3)
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(2).Value = "H: " & My.Settings.Feasa3HueMin3 & " & " & "I: " & My.Settings.Feasa3IntenMin3
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(3).Value = "H: " & My.Settings.Feasa3HueMax3 & " & " & "I: " & My.Settings.Feasa3IntenMax3
                    DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(4).Value = "H: " & Feasa.Value3_Hue3 & " & " & "I: " & Feasa.Value3_Inten3
                    If ComboBoxModel.Text = "CG" Or ComboBoxModel.Text = "DG" Or ComboBoxModel.Text = "JG" Or ComboBoxModel.Text = "KG" Or
                        ComboBoxModel.Text = "CH" Or ComboBoxModel.Text = "DH" Or ComboBoxModel.Text = "JH" Or ComboBoxModel.Text = "KH" Then
                        If Feasa.Condition3_Hue3 = False And Feasa.Condition3_Inten3 = False Then
                            DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                            DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK SMART NEST 3|F|CHECK SMART NEST 3|||"
                            DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                            DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                            ProcessN3 = False
                        End If
                    Else
                        If Feasa.Condition3_Hue3 = True And Feasa.Condition3_Inten3 = True Then
                            DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                            DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK SMART NEST 3|F|CHECK SMART NEST 3|||"
                            DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                            DataGridViewProcessNest3.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                            ProcessN3 = False
                        End If
                    End If

                    '====== HIGH BEAM ======'
                    CheckPoint = ItemCheck(4)
                    DataGridViewProcessNest3.Rows.Item(StepTest).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest3.Rows.Item(StepTest).Cells(2).Value = "H: " & My.Settings.Feasa3HueMin4 & " & " & "I: " & My.Settings.Feasa3IntenMin4
                    DataGridViewProcessNest3.Rows.Item(StepTest).Cells(3).Value = "H: " & My.Settings.Feasa3HueMax4 & " & " & "I: " & My.Settings.Feasa3IntenMax4
                    DataGridViewProcessNest3.Rows.Item(StepTest).Cells(4).Value = "H: " & Feasa.Value3_Hue4 & " & " & "I: " & Feasa.Value3_Inten4
                    If Feasa.Condition3_Hue4 = True And Feasa.Condition3_Inten4 = True Then
                        DataGridViewProcessNest3.Rows.Item(StepTest).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK HIGH BEAM NEST 3|F|CHECK HIGH BEAM NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                    End If

                    '====== ENGINE ======'
                    CheckPoint = ItemCheck(5)
                    DataGridViewProcessNest3.Rows.Item(StepTest + 1).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest3.Rows.Item(StepTest + 1).Cells(2).Value = "H: " & My.Settings.Feasa3HueMin5 & " & " & "I: " & My.Settings.Feasa3IntenMin5
                    DataGridViewProcessNest3.Rows.Item(StepTest + 1).Cells(3).Value = "H: " & My.Settings.Feasa3HueMax5 & " & " & "I: " & My.Settings.Feasa3IntenMax5
                    DataGridViewProcessNest3.Rows.Item(StepTest + 1).Cells(4).Value = "H: " & Feasa.Value3_Hue5 & " & " & "I: " & Feasa.Value3_Inten5
                    If Feasa.Condition3_Hue5 = True And Feasa.Condition3_Inten5 = True Then
                        DataGridViewProcessNest3.Rows.Item(StepTest + 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest + 1).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK ENGINE NEST 3|F|CHECK ENGINE NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest + 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest + 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                    End If

                    '====== TW ======'
                    CheckPoint = ItemCheck(6)
                    DataGridViewProcessNest3.Rows.Item(StepTest + 2).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest3.Rows.Item(StepTest + 2).Cells(2).Value = "H: " & My.Settings.Feasa3HueMin6 & " & " & "I: " & My.Settings.Feasa3IntenMin6
                    DataGridViewProcessNest3.Rows.Item(StepTest + 2).Cells(3).Value = "H: " & My.Settings.Feasa3HueMax6 & " & " & "I: " & My.Settings.Feasa3IntenMax6
                    DataGridViewProcessNest3.Rows.Item(StepTest + 2).Cells(4).Value = "H: " & Feasa.Value3_Hue6 & " & " & "I: " & Feasa.Value3_Inten6
                    If Feasa.Condition3_Hue6 = True And Feasa.Condition3_Inten6 = True Then
                        DataGridViewProcessNest3.Rows.Item(StepTest + 2).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest + 2).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK TW NEST 3|F|CHECK TW NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest + 2).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest + 2).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                    End If

                    '====== ISS ======'
                    CheckPoint = ItemCheck(7)
                    DataGridViewProcessNest3.Rows.Item(StepTest + 3).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest3.Rows.Item(StepTest + 3).Cells(2).Value = "H: " & My.Settings.Feasa3HueMin7 & " & " & "I: " & My.Settings.Feasa3IntenMin7
                    DataGridViewProcessNest3.Rows.Item(StepTest + 3).Cells(3).Value = "H: " & My.Settings.Feasa3HueMax7 & " & " & "I: " & My.Settings.Feasa3IntenMax7
                    DataGridViewProcessNest3.Rows.Item(StepTest + 3).Cells(4).Value = "H: " & Feasa.Value3_Hue7 & " & " & "I: " & Feasa.Value3_Inten7
                    If ComboBoxModel.Text = "DG" Or ComboBoxModel.Text = "KG" Or
                        ComboBoxModel.Text = "DH" Or ComboBoxModel.Text = "KH" Then
                        If Feasa.Condition3_Hue7 = False And Feasa.Condition3_Inten7 = False Then
                            DataGridViewProcessNest3.Rows.Item(StepTest + 3).Cells(5).Value = "OK"
                            DataGridViewProcessNest3.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ISS NEST 3|F|CHECK ISS NEST 3|||"
                            DataGridViewProcessNest3.Rows.Item(StepTest + 3).Cells(5).Value = "NG"
                            DataGridViewProcessNest3.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.Red
                            ProcessN3 = False
                        End If
                    Else
                        If Feasa.Condition3_Hue7 = True And Feasa.Condition3_Inten7 = True Then
                            DataGridViewProcessNest3.Rows.Item(StepTest + 3).Cells(5).Value = "OK"
                            DataGridViewProcessNest3.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ISS NEST 3|F|CHECK ISS NEST 3|||"
                            DataGridViewProcessNest3.Rows.Item(StepTest + 3).Cells(5).Value = "NG"
                            DataGridViewProcessNest3.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.Red
                            ProcessN3 = False
                        End If
                    End If

                    '====== BATTERY FAIL ======'
                    CheckPoint = ItemCheck(8)
                    DataGridViewProcessNest3.Rows.Item(StepTest + 4).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest3.Rows.Item(StepTest + 4).Cells(2).Value = "H: " & My.Settings.Feasa3HueMin8 & " & " & "I: " & My.Settings.Feasa3IntenMin8
                    DataGridViewProcessNest3.Rows.Item(StepTest + 4).Cells(3).Value = "H: " & My.Settings.Feasa3HueMax8 & " & " & "I: " & My.Settings.Feasa3IntenMax8
                    DataGridViewProcessNest3.Rows.Item(StepTest + 4).Cells(4).Value = "H: " & Feasa.Value3_Hue8 & " & " & "I: " & Feasa.Value3_Inten8
                    If Feasa.Condition3_Hue8 = True And Feasa.Condition3_Inten8 = True Then
                        DataGridViewProcessNest3.Rows.Item(StepTest + 4).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest + 4).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK BATTERY FAIL NEST 3|F|CHECK BATTERY FAIL NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest + 4).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest + 4).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                    End If

                    '====== TURN RIGHT ======'
                    CheckPoint = ItemCheck(9)
                    DataGridViewProcessNest3.Rows.Item(StepTest + 5).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest3.Rows.Item(StepTest + 5).Cells(2).Value = "H: " & My.Settings.Feasa3HueMin9 & " & " & "I: " & My.Settings.Feasa3IntenMin9
                    DataGridViewProcessNest3.Rows.Item(StepTest + 5).Cells(3).Value = "H: " & My.Settings.Feasa3HueMax9 & " & " & "I: " & My.Settings.Feasa3IntenMax9
                    DataGridViewProcessNest3.Rows.Item(StepTest + 5).Cells(4).Value = "H: " & Feasa.Value3_Hue9 & " & " & "I: " & Feasa.Value3_Inten9
                    If Feasa.Condition3_Hue9 = True And Feasa.Condition3_Inten9 = True Then
                        DataGridViewProcessNest3.Rows.Item(StepTest + 5).Cells(5).Value = "OK"
                        DataGridViewProcessNest3.Rows.Item(StepTest + 5).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK TURN RIGHT NEST 3|F|CHECK TURN RIGHT NEST 3|||"
                        DataGridViewProcessNest3.Rows.Item(StepTest + 5).Cells(5).Value = "NG"
                        DataGridViewProcessNest3.Rows.Item(StepTest + 5).Cells(5).Style.BackColor = Color.Red
                        ProcessN3 = False
                    End If

                    '====== ALARM ======'
                    CheckPoint = ItemCheck(10)
                    DataGridViewProcessNest3.Rows.Item(StepTest + 6).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest3.Rows.Item(StepTest + 6).Cells(2).Value = "H: " & My.Settings.Feasa3HueMin10 & " & " & "I: " & My.Settings.Feasa3IntenMin10
                    DataGridViewProcessNest3.Rows.Item(StepTest + 6).Cells(3).Value = "H: " & My.Settings.Feasa3HueMax10 & " & " & "I: " & My.Settings.Feasa3IntenMax10
                    DataGridViewProcessNest3.Rows.Item(StepTest + 6).Cells(4).Value = "H: " & Feasa.Value3_Hue10 & " & " & "I: " & Feasa.Value3_Inten10
                    If ComboBoxModel.Text = "DG" Or ComboBoxModel.Text = "KG" Or
                        ComboBoxModel.Text = "DH" Or ComboBoxModel.Text = "KH" Then
                        If Feasa.Condition3_Hue10 = False And Feasa.Condition3_Inten10 = False Then
                            DataGridViewProcessNest3.Rows.Item(StepTest + 6).Cells(5).Value = "OK"
                            DataGridViewProcessNest3.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ALARM NEST 3|F|CHECK ALARM NEST 3|||"
                            DataGridViewProcessNest3.Rows.Item(StepTest + 6).Cells(5).Value = "NG"
                            DataGridViewProcessNest3.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.Red
                            ProcessN3 = False
                        End If
                    Else
                        If Feasa.Condition3_Hue10 = True And Feasa.Condition3_Inten10 = True Then
                            DataGridViewProcessNest3.Rows.Item(StepTest + 6).Cells(5).Value = "OK"
                            DataGridViewProcessNest3.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ALARM NEST 3|F|CHECK ALARM NEST 3|||"
                            DataGridViewProcessNest3.Rows.Item(StepTest + 6).Cells(5).Value = "NG"
                            DataGridViewProcessNest3.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.Red
                            ProcessN3 = False
                        End If
                    End If

                    If ProcessN3 = False Then
                        Nest3NG()
                    End If
                End If

                '====== NEST 4 ======'
                If ProcessN4 = True Then
                    DataGridViewProcessNest4.FirstDisplayedScrollingRowIndex = 7
                    ProgressBarProcess.Value = StepTest * 1.75

                    '====== TURN LEFT ======'
                    CheckPoint = ItemCheck(2)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 2).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest4.Rows.Item(StepTest - 2).Cells(2).Value = "H: " & My.Settings.Feasa4HueMin2 & " & " & "I: " & My.Settings.Feasa4IntenMin2
                    DataGridViewProcessNest4.Rows.Item(StepTest - 2).Cells(3).Value = "H: " & My.Settings.Feasa4HueMax2 & " & " & "I: " & My.Settings.Feasa4IntenMax2
                    DataGridViewProcessNest4.Rows.Item(StepTest - 2).Cells(4).Value = "H: " & Feasa.Value4_Hue2 & " & " & "I: " & Feasa.Value4_Inten2
                    If Feasa.Condition4_Hue2 = True And Feasa.Condition4_Inten2 = True Then
                        DataGridViewProcessNest4.Rows.Item(StepTest - 2).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 2).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK TURN LEFT NEST 4|F|CHECK TURN LEFT NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 2).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest - 2).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                    End If

                    '====== SMART ======'
                    CheckPoint = ItemCheck(3)
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(2).Value = "H: " & My.Settings.Feasa4HueMin3 & " & " & "I: " & My.Settings.Feasa4IntenMin3
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(3).Value = "H: " & My.Settings.Feasa4HueMax3 & " & " & "I: " & My.Settings.Feasa4IntenMax3
                    DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(4).Value = "H: " & Feasa.Value4_Hue3 & " & " & "I: " & Feasa.Value4_Inten3
                    If ComboBoxModel.Text = "CG" Or ComboBoxModel.Text = "DG" Or ComboBoxModel.Text = "JG" Or ComboBoxModel.Text = "KG" Or
                        ComboBoxModel.Text = "CH" Or ComboBoxModel.Text = "DH" Or ComboBoxModel.Text = "JH" Or ComboBoxModel.Text = "KH" Then
                        If Feasa.Condition4_Hue3 = False And Feasa.Condition4_Inten3 = False Then
                            DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                            DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK SMART NEST 4|F|CHECK SMART NEST 4|||"
                            DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                            DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                            ProcessN4 = False
                        End If
                    Else
                        If Feasa.Condition4_Hue3 = True And Feasa.Condition4_Inten3 = True Then
                            DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "OK"
                            DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK SMART NEST 4|F|CHECK SMART NEST 4|||"
                            DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Value = "NG"
                            DataGridViewProcessNest4.Rows.Item(StepTest - 1).Cells(5).Style.BackColor = Color.Red
                            ProcessN4 = False
                        End If
                    End If

                    '====== HIGH BEAM ======'
                    CheckPoint = ItemCheck(4)
                    DataGridViewProcessNest4.Rows.Item(StepTest).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest4.Rows.Item(StepTest).Cells(2).Value = "H: " & My.Settings.Feasa4HueMin4 & " & " & "I: " & My.Settings.Feasa4IntenMin4
                    DataGridViewProcessNest4.Rows.Item(StepTest).Cells(3).Value = "H: " & My.Settings.Feasa4HueMax4 & " & " & "I: " & My.Settings.Feasa4IntenMax4
                    DataGridViewProcessNest4.Rows.Item(StepTest).Cells(4).Value = "H: " & Feasa.Value4_Hue4 & " & " & "I: " & Feasa.Value4_Inten4
                    If Feasa.Condition4_Hue4 = True And Feasa.Condition4_Inten4 = True Then
                        DataGridViewProcessNest4.Rows.Item(StepTest).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK HIGH BEAM NEST 4|F|CHECK HIGH BEAM NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                    End If

                    '====== ENGINE ======'
                    CheckPoint = ItemCheck(5)
                    DataGridViewProcessNest4.Rows.Item(StepTest + 1).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest4.Rows.Item(StepTest + 1).Cells(2).Value = "H: " & My.Settings.Feasa4HueMin5 & " & " & "I: " & My.Settings.Feasa4IntenMin5
                    DataGridViewProcessNest4.Rows.Item(StepTest + 1).Cells(3).Value = "H: " & My.Settings.Feasa4HueMax5 & " & " & "I: " & My.Settings.Feasa4IntenMax5
                    DataGridViewProcessNest4.Rows.Item(StepTest + 1).Cells(4).Value = "H: " & Feasa.Value4_Hue5 & " & " & "I: " & Feasa.Value4_Inten5
                    If Feasa.Condition4_Hue5 = True And Feasa.Condition4_Inten5 = True Then
                        DataGridViewProcessNest4.Rows.Item(StepTest + 1).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest + 1).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK ENGINE NEST 4|F|CHECK ENGINE NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest + 1).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest + 1).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                    End If

                    '====== TW ======'
                    CheckPoint = ItemCheck(6)
                    DataGridViewProcessNest4.Rows.Item(StepTest + 2).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest4.Rows.Item(StepTest + 2).Cells(2).Value = "H: " & My.Settings.Feasa4HueMin6 & " & " & "I: " & My.Settings.Feasa4IntenMin6
                    DataGridViewProcessNest4.Rows.Item(StepTest + 2).Cells(3).Value = "H: " & My.Settings.Feasa4HueMax6 & " & " & "I: " & My.Settings.Feasa4IntenMax6
                    DataGridViewProcessNest4.Rows.Item(StepTest + 2).Cells(4).Value = "H: " & Feasa.Value4_Hue6 & " & " & "I: " & Feasa.Value4_Inten6
                    If Feasa.Condition4_Hue6 = True And Feasa.Condition4_Inten6 = True Then
                        DataGridViewProcessNest4.Rows.Item(StepTest + 2).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest + 2).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK TW NEST 4|F|CHECK TW NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest + 2).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest + 2).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                    End If

                    '====== ISS ======'
                    CheckPoint = ItemCheck(7)
                    DataGridViewProcessNest4.Rows.Item(StepTest + 3).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest4.Rows.Item(StepTest + 3).Cells(2).Value = "H: " & My.Settings.Feasa4HueMin7 & " & " & "I: " & My.Settings.Feasa4IntenMin7
                    DataGridViewProcessNest4.Rows.Item(StepTest + 3).Cells(3).Value = "H: " & My.Settings.Feasa4HueMax7 & " & " & "I: " & My.Settings.Feasa4IntenMax7
                    DataGridViewProcessNest4.Rows.Item(StepTest + 3).Cells(4).Value = "H: " & Feasa.Value4_Hue7 & " & " & "I: " & Feasa.Value4_Inten7
                    If ComboBoxModel.Text = "DG" Or ComboBoxModel.Text = "KG" Or
                        ComboBoxModel.Text = "DH" Or ComboBoxModel.Text = "KH" Then
                        If Feasa.Condition4_Hue7 = False And Feasa.Condition4_Inten7 = False Then
                            DataGridViewProcessNest4.Rows.Item(StepTest + 3).Cells(5).Value = "OK"
                            DataGridViewProcessNest4.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ISS NEST 4|F|CHECK ISS NEST 4|||"
                            DataGridViewProcessNest4.Rows.Item(StepTest + 3).Cells(5).Value = "NG"
                            DataGridViewProcessNest4.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.Red
                            ProcessN4 = False
                        End If
                    Else
                        If Feasa.Condition4_Hue7 = True And Feasa.Condition4_Inten7 = True Then
                            DataGridViewProcessNest4.Rows.Item(StepTest + 3).Cells(5).Value = "OK"
                            DataGridViewProcessNest4.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ISS NEST 4|F|CHECK ISS NEST 4|||"
                            DataGridViewProcessNest4.Rows.Item(StepTest + 3).Cells(5).Value = "NG"
                            DataGridViewProcessNest4.Rows.Item(StepTest + 3).Cells(5).Style.BackColor = Color.Red
                            ProcessN4 = False
                        End If
                    End If

                    '====== BATTERY FAIL ======'
                    CheckPoint = ItemCheck(8)
                    DataGridViewProcessNest4.Rows.Item(StepTest + 4).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest4.Rows.Item(StepTest + 4).Cells(2).Value = "H: " & My.Settings.Feasa4HueMin8 & " & " & "I: " & My.Settings.Feasa4IntenMin8
                    DataGridViewProcessNest4.Rows.Item(StepTest + 4).Cells(3).Value = "H: " & My.Settings.Feasa4HueMax8 & " & " & "I: " & My.Settings.Feasa4IntenMax8
                    DataGridViewProcessNest4.Rows.Item(StepTest + 4).Cells(4).Value = "H: " & Feasa.Value4_Hue8 & " & " & "I: " & Feasa.Value4_Inten8
                    If Feasa.Condition4_Hue8 = True And Feasa.Condition4_Inten8 = True Then
                        DataGridViewProcessNest4.Rows.Item(StepTest + 4).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest + 4).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK BATTERY FAIL NEST 4|F|CHECK BATTERY FAIL NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest + 4).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest + 4).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                    End If

                    '====== TURN RIGHT ======'
                    CheckPoint = ItemCheck(9)
                    DataGridViewProcessNest4.Rows.Item(StepTest + 5).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest4.Rows.Item(StepTest + 5).Cells(2).Value = "H: " & My.Settings.Feasa4HueMin9 & " & " & "I: " & My.Settings.Feasa4IntenMin9
                    DataGridViewProcessNest4.Rows.Item(StepTest + 5).Cells(3).Value = "H: " & My.Settings.Feasa4HueMax9 & " & " & "I: " & My.Settings.Feasa4IntenMax9
                    DataGridViewProcessNest4.Rows.Item(StepTest + 5).Cells(4).Value = "H: " & Feasa.Value4_Hue9 & " & " & "I: " & Feasa.Value4_Inten9
                    If Feasa.Condition4_Hue9 = True And Feasa.Condition4_Inten9 = True Then
                        DataGridViewProcessNest4.Rows.Item(StepTest + 5).Cells(5).Value = "OK"
                        DataGridViewProcessNest4.Rows.Item(StepTest + 5).Cells(5).Style.BackColor = Color.GreenYellow
                    Else
                        NGType = "|*|FUN|*|1|1|CHECK TURN RIGHT NEST 4|F|CHECK TURN RIGHT NEST 4|||"
                        DataGridViewProcessNest4.Rows.Item(StepTest + 5).Cells(5).Value = "NG"
                        DataGridViewProcessNest4.Rows.Item(StepTest + 5).Cells(5).Style.BackColor = Color.Red
                        ProcessN4 = False
                    End If

                    '====== ALARM ======'
                    CheckPoint = ItemCheck(10)
                    DataGridViewProcessNest4.Rows.Item(StepTest + 6).Cells(1).Value = CheckPoint
                    DataGridViewProcessNest4.Rows.Item(StepTest + 6).Cells(2).Value = "H: " & My.Settings.Feasa4HueMin10 & " & " & "I: " & My.Settings.Feasa4IntenMin10
                    DataGridViewProcessNest4.Rows.Item(StepTest + 6).Cells(3).Value = "H: " & My.Settings.Feasa4HueMax10 & " & " & "I: " & My.Settings.Feasa4IntenMax10
                    DataGridViewProcessNest4.Rows.Item(StepTest + 6).Cells(4).Value = "H: " & Feasa.Value4_Hue10 & " & " & "I: " & Feasa.Value4_Inten10
                    If ComboBoxModel.Text = "DG" Or ComboBoxModel.Text = "KG" Or
                        ComboBoxModel.Text = "DH" Or ComboBoxModel.Text = "KH" Then
                        If Feasa.Condition4_Hue10 = False And Feasa.Condition4_Inten10 = False Then
                            DataGridViewProcessNest4.Rows.Item(StepTest + 6).Cells(5).Value = "OK"
                            DataGridViewProcessNest4.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ALARM NEST 4|F|CHECK ALARM NEST 4|||"
                            DataGridViewProcessNest4.Rows.Item(StepTest + 6).Cells(5).Value = "NG"
                            DataGridViewProcessNest4.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.Red
                            ProcessN4 = False
                        End If
                    Else
                        If Feasa.Condition4_Hue10 = True And Feasa.Condition4_Inten10 = True Then
                            DataGridViewProcessNest4.Rows.Item(StepTest + 6).Cells(5).Value = "OK"
                            DataGridViewProcessNest4.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.GreenYellow
                        Else
                            NGType = "|*|FUN|*|1|1|CHECK ALARM NEST 4|F|CHECK ALARM NEST 4|||"
                            DataGridViewProcessNest4.Rows.Item(StepTest + 6).Cells(5).Value = "NG"
                            DataGridViewProcessNest4.Rows.Item(StepTest + 6).Cells(5).Style.BackColor = Color.Red
                            ProcessN4 = False
                        End If
                    End If

                    If ProcessN4 = False Then
                        Nest4NG()
                    End If
                End If

                StepTest += 1

            Case Else
                ProgressBarProcess.Value = 100

                LabelProses.BackColor = Color.Green
                LabelProses.Text = "FINISH"

                ModelSelectN1 = False
                ModelSelectN2 = False
                ModelSelectN3 = False
                ModelSelectN4 = False
                StepTest = 1000

                WriteReport()

                If PushButtonEmergency = False Then
                    TextBoxBarcodeN1.Text = "Emergency Pressed"

                ElseIf ProcessN1 = False And RunningNest1 = True Then
                    CounterNGN1 = My.Settings.CounterNGN1
                    CounterNGN1 = CounterNGN1 + 1
                    My.Settings.CounterNGN1 = CounterNGN1
                    My.Settings.Save()
                    LabelCounterNGN1.Text = My.Settings.CounterNGN1
                    JigN1On()
                    TextBoxBarcodeN1.Text = "Press Emergency"

                ElseIf ProcessN1 = True And RunningNest1 = True Then
                    JigN1Off()

                    'If CheckBoxSendCIM.Checked = True Then
                    '    Write100N1()
                    '    Delay_Ms(100)
                    'Else
                    '    NoCimOkN1()
                    'End If

                    If CheckBoxSendCIM.Checked = True Then
                        Write100N1()
                        Delay_Ms(200)
                    End If

                    LabelJudgementN1.BackColor = Color.Lime
                    LabelJudgementN1.Text = "PASS"

                    CounterOKN1 = My.Settings.CounterOKN1
                    CounterOKN1 = CounterOKN1 + 1
                    My.Settings.CounterOKN1 = CounterOKN1
                    My.Settings.Save()

                    LabelCounterOKN1.Text = My.Settings.CounterOKN1
                End If

                If PushButtonEmergency = False Then
                    TextBoxBarcodeN2.Text = "Emergency Pressed"

                ElseIf ProcessN2 = False And RunningNest2 = True Then
                    CounterNGN2 = My.Settings.CounterNGN2
                    CounterNGN2 = CounterNGN2 + 1
                    My.Settings.CounterNGN2 = CounterNGN2
                    My.Settings.Save()
                    LabelCounterNGN2.Text = My.Settings.CounterNGN2
                    JigN2On()
                    TextBoxBarcodeN2.Text = "Press Emergency"

                ElseIf ProcessN2 = True And RunningNest2 = True Then
                    JigN2Off()

                    'If CheckBoxSendCIM.Checked = True Then
                    '    Write100N2()
                    '    Delay_Ms(100)
                    'Else
                    '    NoCimOkN2()
                    'End If

                    If CheckBoxSendCIM.Checked = True Then
                        Write100N2()
                        Delay_Ms(400)
                    End If

                    LabelJudgementN2.BackColor = Color.Lime
                    LabelJudgementN2.Text = "PASS"

                    CounterOKN2 = My.Settings.CounterOKN2
                    CounterOKN2 = CounterOKN2 + 1
                    My.Settings.CounterOKN2 = CounterOKN2
                    My.Settings.Save()

                    LabelCounterOKN2.Text = My.Settings.CounterOKN2
                End If

                If PushButtonEmergency = False Then
                    TextBoxBarcodeN3.Text = "Emergency Pressed"

                ElseIf ProcessN3 = False And RunningNest3 = True Then
                    CounterNGN3 = My.Settings.CounterNGN3
                    CounterNGN3 = CounterNGN3 + 1
                    My.Settings.CounterNGN3 = CounterNGN3
                    My.Settings.Save()
                    LabelCounterNGN3.Text = My.Settings.CounterNGN3
                    JigN3On()
                    TextBoxBarcodeN3.Text = "Press Emergency"

                ElseIf ProcessN3 = True And RunningNest3 = True Then
                    JigN3Off()

                    'If CheckBoxSendCIM.Checked = True Then
                    '    Write100N3()
                    '    Delay_Ms(100)
                    'Else
                    '    NoCimOkN3()
                    'End If

                    If CheckBoxSendCIM.Checked = True Then
                        Write100N3()
                        Delay_Ms(600)
                    End If

                    LabelJudgementN3.BackColor = Color.Lime
                    LabelJudgementN3.Text = "PASS"

                    CounterOKN3 = My.Settings.CounterOKN3
                    CounterOKN3 = CounterOKN3 + 1
                    My.Settings.CounterOKN3 = CounterOKN3
                    My.Settings.Save()

                    LabelCounterOKN3.Text = My.Settings.CounterOKN3
                End If

                If PushButtonEmergency = False Then
                    TextBoxBarcodeN4.Text = "Emergency Pressed"

                ElseIf ProcessN4 = False And RunningNest4 = True Then
                    CounterNGN4 = My.Settings.CounterNGN4
                    CounterNGN4 = CounterNGN4 + 1
                    My.Settings.CounterNGN4 = CounterNGN4
                    My.Settings.Save()
                    LabelCounterNGN4.Text = My.Settings.CounterNGN4
                    JigN4On()
                    TextBoxBarcodeN4.Text = "Press Emergency"

                ElseIf ProcessN4 = True And RunningNest4 = True Then
                    JigN4Off()

                    'If CheckBoxSendCIM.Checked = True Then
                    '    Write100N4()
                    '    Delay_Ms(100)
                    'Else
                    '    NoCimOkN4()
                    'End If

                    If CheckBoxSendCIM.Checked = True Then
                        Write100N4()
                        Delay_Ms(800)
                    End If

                    LabelJudgementN4.BackColor = Color.Lime
                    LabelJudgementN4.Text = "PASS"

                    CounterOKN4 = My.Settings.CounterOKN4
                    CounterOKN4 = CounterOKN4 + 1
                    My.Settings.CounterOKN4 = CounterOKN4
                    My.Settings.Save()

                    LabelCounterOKN4.Text = My.Settings.CounterOKN4
                End If

                'NGRateN1 = CounterNGN1 / ((CounterNGN1 + CounterOKN1))
                'NGRateN2 = CounterNGN2 / ((CounterNGN2 + CounterOKN2))
                'NGRateN3 = CounterNGN3 / ((CounterNGN3 + CounterOKN3))
                'NGRateN4 = CounterNGN4 / ((CounterNGN4 + CounterOKN4))

                Dim Elapsed As TimeSpan = Me.Stopwatch.Elapsed
                LabelMachineTime.Text = String.Format("{0:00}:{1:00}:{2:00}:{3:00}", Math.Floor(Elapsed.TotalHours), Elapsed.Minutes, Elapsed.Seconds, Elapsed.Milliseconds)

                Timer3.Stop()
                Me.Stopwatch.Stop()

                BootmodeN1Off()
                BootmodeN2Off()
                BootmodeN3Off()
                BootmodeN4Off()

                AllLEDN1Off()
                AllLEDN2Off()
                AllLEDN3Off()
                AllLEDN4Off()

                PSU15V()
                Delay_Ms(25)

                TextBoxBarcode.Clear()

                TextBoxBarcodeN1.Clear()
                TextBoxBarcodeN2.Clear()
                TextBoxBarcodeN3.Clear()
                TextBoxBarcodeN4.Clear()

                TextBoxBarcodeN1.BackColor = Color.White
                TextBoxBarcodeN2.BackColor = Color.White
                TextBoxBarcodeN3.BackColor = Color.White
                TextBoxBarcodeN4.BackColor = Color.White

                BarcodeN1 = ""
                BarcodeN2 = ""
                BarcodeN3 = ""
                BarcodeN4 = ""

                CaseElseCondition = True
                StepTest = 0
                CountLoop = 0
        End Select
    End Sub

    '===== LOG FILE =====

    Public Sub WriteReport()
        Day = Date.Now.Day
        Month = Date.Now.Month
        Print = Date.Now.Month
        Year = Date.Now.Year
        FileDir = "C:\Record_FCT_K2VG_K59J\" & Year & Month & Day

        'NEST 1
        For i = 0 To DataGridViewProcessNest1.Rows.Count - 1
            TextAdd1 = ""
            TextAdd2 = ""
            TextAdd3 = ""
            TextAdd4 = ""
            TextAdd5 = ""
            For l = 1 To 6
                TextAdd(l) = ""
            Next
            For j = 1 To 5
                LenTextMaster = 0
                LenTextAdd = 0
                TextMaster = DataGridViewProcessNest1.Rows.Item(i).Cells(j).Value
                Select Case j
                    Case 1
                        SelectFor = 30
                    Case 2
                        SelectFor = 20
                    Case 3
                        SelectFor = 30
                    Case 4
                        SelectFor = 10
                    Case 5
                        SelectFor = 10
                End Select
                LenTextMaster = Len(TextMaster)
                LenTextAdd = SelectFor - LenTextMaster
                Select Case j
                    Case 1
                        For k = 1 To LenTextAdd
                            TextAdd1 = TextAdd1 & "."
                        Next
                    Case 2
                        For k = 1 To LenTextAdd
                            TextAdd2 = TextAdd2 & "."
                        Next
                    Case 3
                        For k = 1 To LenTextAdd
                            TextAdd3 = TextAdd3 & "."
                        Next
                    Case 4
                        For k = 1 To LenTextAdd
                            TextAdd4 = TextAdd4 & "."
                        Next
                    Case 5
                        For k = 1 To LenTextAdd
                            TextAdd5 = TextAdd5 & "."
                        Next
                End Select
            Next j

            If Len(Dir(FileDir, vbDirectory)) = 0 Then
                MkDir(FileDir)
                FileOpen(File, FileDir & "\" & BarcodeN1 & ".txt", OpenMode.Append)
                If i < 10 Then
                    PrintLine(File, DataGridViewProcessNest1.Rows.Item(i).Cells(0).Value & " -- " & DataGridViewProcessNest1.Rows.Item(i).Cells(1).Value _
                      & TextAdd1 & DataGridViewProcessNest1.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest1.Rows.Item(i).Cells(3).Value _
                     & TextAdd3 & DataGridViewProcessNest1.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest1.Rows.Item(i).Cells(5).Value)
                Else
                    PrintLine(File, DataGridViewProcessNest1.Rows.Item(i).Cells(0).Value & " - " & DataGridViewProcessNest1.Rows.Item(i).Cells(1).Value _
                     & TextAdd1 & DataGridViewProcessNest1.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest1.Rows.Item(i).Cells(3).Value _
                    & TextAdd3 & DataGridViewProcessNest1.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest1.Rows.Item(i).Cells(5).Value)
                End If

                FileClose(File)
            Else
                FileOpen(File, FileDir & "\" & BarcodeN1 & ".txt", OpenMode.Append)
                If i < 10 Then
                    PrintLine(File, DataGridViewProcessNest1.Rows.Item(i).Cells(0).Value & " -- " & DataGridViewProcessNest1.Rows.Item(i).Cells(1).Value _
                      & TextAdd1 & DataGridViewProcessNest1.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest1.Rows.Item(i).Cells(3).Value _
                     & TextAdd3 & DataGridViewProcessNest1.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest1.Rows.Item(i).Cells(5).Value)
                Else
                    PrintLine(File, DataGridViewProcessNest1.Rows.Item(i).Cells(0).Value & " - " & DataGridViewProcessNest1.Rows.Item(i).Cells(1).Value _
                     & TextAdd1 & DataGridViewProcessNest1.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest1.Rows.Item(i).Cells(3).Value _
                    & TextAdd3 & DataGridViewProcessNest1.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest1.Rows.Item(i).Cells(5).Value)
                End If
                FileClose(File)
            End If
        Next i

        'NEST 2
        For i = 0 To DataGridViewProcessNest2.Rows.Count - 1
            TextAdd1 = ""
            TextAdd2 = ""
            TextAdd3 = ""
            TextAdd4 = ""
            TextAdd5 = ""
            For l = 1 To 6
                TextAdd(l) = ""
            Next
            For j = 1 To 5
                LenTextMaster = 0
                LenTextAdd = 0
                TextMaster = DataGridViewProcessNest2.Rows.Item(i).Cells(j).Value
                Select Case j
                    Case 1
                        SelectFor = 30
                    Case 2
                        SelectFor = 20
                    Case 3
                        SelectFor = 30
                    Case 4
                        SelectFor = 10
                    Case 5
                        SelectFor = 10
                End Select
                LenTextMaster = Len(TextMaster)
                LenTextAdd = SelectFor - LenTextMaster
                Select Case j
                    Case 1
                        For k = 1 To LenTextAdd
                            TextAdd1 = TextAdd1 & "."
                        Next
                    Case 2
                        For k = 1 To LenTextAdd
                            TextAdd2 = TextAdd2 & "."
                        Next
                    Case 3
                        For k = 1 To LenTextAdd
                            TextAdd3 = TextAdd3 & "."
                        Next
                    Case 4
                        For k = 1 To LenTextAdd
                            TextAdd4 = TextAdd4 & "."
                        Next
                    Case 5
                        For k = 1 To LenTextAdd
                            TextAdd5 = TextAdd5 & "."
                        Next
                End Select
            Next j

            If Len(Dir(FileDir, vbDirectory)) = 0 Then
                MkDir(FileDir)
                FileOpen(File, FileDir & "\" & BarcodeN2 & ".txt", OpenMode.Append)
                If i < 10 Then
                    PrintLine(File, DataGridViewProcessNest2.Rows.Item(i).Cells(0).Value & " -- " & DataGridViewProcessNest2.Rows.Item(i).Cells(1).Value _
                      & TextAdd1 & DataGridViewProcessNest2.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest2.Rows.Item(i).Cells(3).Value _
                     & TextAdd3 & DataGridViewProcessNest2.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest2.Rows.Item(i).Cells(5).Value)
                Else
                    PrintLine(File, DataGridViewProcessNest2.Rows.Item(i).Cells(0).Value & " - " & DataGridViewProcessNest2.Rows.Item(i).Cells(1).Value _
                     & TextAdd1 & DataGridViewProcessNest2.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest2.Rows.Item(i).Cells(3).Value _
                    & TextAdd3 & DataGridViewProcessNest2.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest2.Rows.Item(i).Cells(5).Value)
                End If

                FileClose(File)
            Else
                FileOpen(File, FileDir & "\" & BarcodeN2 & ".txt", OpenMode.Append)
                If i < 10 Then
                    PrintLine(File, DataGridViewProcessNest2.Rows.Item(i).Cells(0).Value & " -- " & DataGridViewProcessNest2.Rows.Item(i).Cells(1).Value _
                      & TextAdd1 & DataGridViewProcessNest2.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest2.Rows.Item(i).Cells(3).Value _
                     & TextAdd3 & DataGridViewProcessNest2.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest2.Rows.Item(i).Cells(5).Value)
                Else
                    PrintLine(File, DataGridViewProcessNest2.Rows.Item(i).Cells(0).Value & " - " & DataGridViewProcessNest2.Rows.Item(i).Cells(1).Value _
                     & TextAdd1 & DataGridViewProcessNest2.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest2.Rows.Item(i).Cells(3).Value _
                    & TextAdd3 & DataGridViewProcessNest2.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest2.Rows.Item(i).Cells(5).Value)
                End If
                FileClose(File)
            End If
        Next i

        'NEST 3
        For i = 0 To DataGridViewProcessNest3.Rows.Count - 1
            TextAdd1 = ""
            TextAdd2 = ""
            TextAdd3 = ""
            TextAdd4 = ""
            TextAdd5 = ""
            For l = 1 To 6
                TextAdd(l) = ""
            Next
            For j = 1 To 5
                LenTextMaster = 0
                LenTextAdd = 0
                TextMaster = DataGridViewProcessNest3.Rows.Item(i).Cells(j).Value
                Select Case j
                    Case 1
                        SelectFor = 30
                    Case 2
                        SelectFor = 20
                    Case 3
                        SelectFor = 30
                    Case 4
                        SelectFor = 10
                    Case 5
                        SelectFor = 10
                End Select
                LenTextMaster = Len(TextMaster)
                LenTextAdd = SelectFor - LenTextMaster
                Select Case j
                    Case 1
                        For k = 1 To LenTextAdd
                            TextAdd1 = TextAdd1 & "."
                        Next
                    Case 2
                        For k = 1 To LenTextAdd
                            TextAdd2 = TextAdd2 & "."
                        Next
                    Case 3
                        For k = 1 To LenTextAdd
                            TextAdd3 = TextAdd3 & "."
                        Next
                    Case 4
                        For k = 1 To LenTextAdd
                            TextAdd4 = TextAdd4 & "."
                        Next
                    Case 5
                        For k = 1 To LenTextAdd
                            TextAdd5 = TextAdd5 & "."
                        Next
                End Select
            Next j

            If Len(Dir(FileDir, vbDirectory)) = 0 Then
                MkDir(FileDir)
                FileOpen(File, FileDir & "\" & BarcodeN3 & ".txt", OpenMode.Append)
                If i < 10 Then
                    PrintLine(File, DataGridViewProcessNest3.Rows.Item(i).Cells(0).Value & " -- " & DataGridViewProcessNest3.Rows.Item(i).Cells(1).Value _
                      & TextAdd1 & DataGridViewProcessNest3.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest3.Rows.Item(i).Cells(3).Value _
                     & TextAdd3 & DataGridViewProcessNest3.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest3.Rows.Item(i).Cells(5).Value)
                Else
                    PrintLine(File, DataGridViewProcessNest3.Rows.Item(i).Cells(0).Value & " - " & DataGridViewProcessNest3.Rows.Item(i).Cells(1).Value _
                     & TextAdd1 & DataGridViewProcessNest3.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest3.Rows.Item(i).Cells(3).Value _
                    & TextAdd3 & DataGridViewProcessNest3.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest3.Rows.Item(i).Cells(5).Value)
                End If

                FileClose(File)
            Else
                FileOpen(File, FileDir & "\" & BarcodeN3 & ".txt", OpenMode.Append)
                If i < 10 Then
                    PrintLine(File, DataGridViewProcessNest3.Rows.Item(i).Cells(0).Value & " -- " & DataGridViewProcessNest3.Rows.Item(i).Cells(1).Value _
                      & TextAdd1 & DataGridViewProcessNest3.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest3.Rows.Item(i).Cells(3).Value _
                     & TextAdd3 & DataGridViewProcessNest3.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest3.Rows.Item(i).Cells(5).Value)
                Else
                    PrintLine(File, DataGridViewProcessNest3.Rows.Item(i).Cells(0).Value & " - " & DataGridViewProcessNest3.Rows.Item(i).Cells(1).Value _
                     & TextAdd1 & DataGridViewProcessNest3.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest3.Rows.Item(i).Cells(3).Value _
                    & TextAdd3 & DataGridViewProcessNest3.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest3.Rows.Item(i).Cells(5).Value)
                End If
                FileClose(File)
            End If
        Next i

        'NEST 4
        For i = 0 To DataGridViewProcessNest4.Rows.Count - 1
            TextAdd1 = ""
            TextAdd2 = ""
            TextAdd3 = ""
            TextAdd4 = ""
            TextAdd5 = ""
            For l = 1 To 6
                TextAdd(l) = ""
            Next
            For j = 1 To 5
                LenTextMaster = 0
                LenTextAdd = 0
                TextMaster = DataGridViewProcessNest4.Rows.Item(i).Cells(j).Value
                Select Case j
                    Case 1
                        SelectFor = 30
                    Case 2
                        SelectFor = 20
                    Case 3
                        SelectFor = 30
                    Case 4
                        SelectFor = 10
                    Case 5
                        SelectFor = 10
                End Select
                LenTextMaster = Len(TextMaster)
                LenTextAdd = SelectFor - LenTextMaster
                Select Case j
                    Case 1
                        For k = 1 To LenTextAdd
                            TextAdd1 = TextAdd1 & "."
                        Next
                    Case 2
                        For k = 1 To LenTextAdd
                            TextAdd2 = TextAdd2 & "."
                        Next
                    Case 3
                        For k = 1 To LenTextAdd
                            TextAdd3 = TextAdd3 & "."
                        Next
                    Case 4
                        For k = 1 To LenTextAdd
                            TextAdd4 = TextAdd4 & "."
                        Next
                    Case 5
                        For k = 1 To LenTextAdd
                            TextAdd5 = TextAdd5 & "."
                        Next
                End Select
            Next j

            If Len(Dir(FileDir, vbDirectory)) = 0 Then
                MkDir(FileDir)
                FileOpen(File, FileDir & "\" & BarcodeN4 & ".txt", OpenMode.Append)
                If i < 10 Then
                    PrintLine(File, DataGridViewProcessNest4.Rows.Item(i).Cells(0).Value & " -- " & DataGridViewProcessNest4.Rows.Item(i).Cells(1).Value _
                      & TextAdd1 & DataGridViewProcessNest4.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest4.Rows.Item(i).Cells(3).Value _
                     & TextAdd3 & DataGridViewProcessNest4.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest4.Rows.Item(i).Cells(5).Value)
                Else
                    PrintLine(File, DataGridViewProcessNest4.Rows.Item(i).Cells(0).Value & " - " & DataGridViewProcessNest4.Rows.Item(i).Cells(1).Value _
                     & TextAdd1 & DataGridViewProcessNest4.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest4.Rows.Item(i).Cells(3).Value _
                    & TextAdd3 & DataGridViewProcessNest4.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest4.Rows.Item(i).Cells(5).Value)
                End If

                FileClose(File)
            Else
                FileOpen(File, FileDir & "\" & BarcodeN4 & ".txt", OpenMode.Append)
                If i < 10 Then
                    PrintLine(File, DataGridViewProcessNest4.Rows.Item(i).Cells(0).Value & " -- " & DataGridViewProcessNest4.Rows.Item(i).Cells(1).Value _
                      & TextAdd1 & DataGridViewProcessNest4.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest4.Rows.Item(i).Cells(3).Value _
                     & TextAdd3 & DataGridViewProcessNest4.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest4.Rows.Item(i).Cells(5).Value)
                Else
                    PrintLine(File, DataGridViewProcessNest4.Rows.Item(i).Cells(0).Value & " - " & DataGridViewProcessNest4.Rows.Item(i).Cells(1).Value _
                     & TextAdd1 & DataGridViewProcessNest4.Rows.Item(i).Cells(2).Value & TextAdd2 & DataGridViewProcessNest4.Rows.Item(i).Cells(3).Value _
                    & TextAdd3 & DataGridViewProcessNest4.Rows.Item(i).Cells(4).Value & TextAdd4 & DataGridViewProcessNest4.Rows.Item(i).Cells(5).Value)
                End If
                FileClose(File)
            End If
        Next i

    End Sub

    Public Sub ProcessDate()
        Year = Date.Now.Year
        Month = Date.Now.Month
        Day = Date.Now.Day
        Hour = Date.Now.Hour
        Minute = Date.Now.Minute
        Second = Date.Now.Second

        If Year < 10 Then
            Year = "0" & Year
        End If

        If Month < 10 Then
            Month = "0" & Month
        End If

        If Hour < 10 Then
            Hour = "0" & Hour
        End If

        If Minute < 10 Then
            Minute = "0" & Minute
        End If

        If Second < 10 Then
            Second = "0" & Second
        End If
    End Sub

    Public Sub RecordDataN1()
        ProcessDate()
        FileDir = "C:\Record_FCT_N1_K2SA_K2SG.txt"

        If My.Computer.FileSystem.FileExists(FileDir) Then
        Else
            System.IO.File.Create(FileDir).Dispose()
        End If

        FileOpen(1, FileDir, OpenMode.Append)
        PrintLine(1, CounterOKN1 & "_" & CounterNGN1 & "_" & NGRateN1 & "_" & Year & "-" & Month & "-" & Day & "_" & Hour & ":" & Minute & ":" & Second)
        PrintLine(1, "")

        FileClose()

        '##### BACKUP #####'
        FileDir = "C:\Record_FCT_N1_K2SA_K2SG_Backup.txt"

        If My.Computer.FileSystem.FileExists(FileDir) Then
        Else
            System.IO.File.Create(FileDir).Dispose()
        End If

        FileOpen(1, FileDir, OpenMode.Append)
        PrintLine(1, CounterOKN1 & "_" & CounterNGN1 & "_" & NGRateN1 & "_" & Year & "-" & Month & "-" & Day & "_" & Hour & ":" & Minute & ":" & Second)
        PrintLine(1, "")

        FileClose()
    End Sub

    Public Sub RecordDataN2()
        ProcessDate()
        FileDir = "C:\Record_FCT_N2_K2SA_K2SG.txt"

        If My.Computer.FileSystem.FileExists(FileDir) Then
        Else
            System.IO.File.Create(FileDir).Dispose()
        End If

        FileOpen(1, FileDir, OpenMode.Append)
        PrintLine(1, CounterOKN2 & "_" & CounterNGN2 & "_" & NGRateN2 & "_" & Year & "-" & Month & "-" & Day & "_" & Hour & ":" & Minute & ":" & Second)
        PrintLine(1, "")

        FileClose()

        '##### BACKUP #####'
        FileDir = "C:\Record_FCT_N2_K2SA_K2SG_Backup.txt"

        If My.Computer.FileSystem.FileExists(FileDir) Then
        Else
            System.IO.File.Create(FileDir).Dispose()
        End If

        FileOpen(1, FileDir, OpenMode.Append)
        PrintLine(1, CounterOKN2 & "_" & CounterNGN2 & "_" & NGRateN2 & "_" & Year & "-" & Month & "-" & Day & "_" & Hour & ":" & Minute & ":" & Second)
        PrintLine(1, "")

        FileClose()
    End Sub

    Public Sub RecordDataN3()
        ProcessDate()
        FileDir = "C:\Record_FCT_N3_K2SA_K2SG.txt"

        If My.Computer.FileSystem.FileExists(FileDir) Then
        Else
            System.IO.File.Create(FileDir).Dispose()
        End If

        FileOpen(1, FileDir, OpenMode.Append)
        PrintLine(1, CounterOKN3 & "_" & CounterNGN3 & "_" & NGRateN3 & "_" & Year & "-" & Month & "-" & Day & "_" & Hour & ":" & Minute & ":" & Second)
        PrintLine(1, "")

        FileClose()

        '##### BACKUP #####'
        FileDir = "C:\Record_FCT_N3_K2SA_K2SG_Backup.txt"

        If My.Computer.FileSystem.FileExists(FileDir) Then
        Else
            System.IO.File.Create(FileDir).Dispose()
        End If

        FileOpen(1, FileDir, OpenMode.Append)
        PrintLine(1, CounterOKN3 & "_" & CounterNGN3 & "_" & NGRateN3 & "_" & Year & "-" & Month & "-" & Day & "_" & Hour & ":" & Minute & ":" & Second)
        PrintLine(1, "")

        FileClose()
    End Sub

    Public Sub RecordDataN4()
        ProcessDate()
        FileDir = "C:\Record_FCT_N4_K2SA_K2SG.txt"

        If My.Computer.FileSystem.FileExists(FileDir) Then
        Else
            System.IO.File.Create(FileDir).Dispose()
        End If

        FileOpen(1, FileDir, OpenMode.Append)
        PrintLine(1, CounterOKN4 & "_" & CounterNGN4 & "_" & NGRateN4 & "_" & Year & "-" & Month & "-" & Day & "_" & Hour & ":" & Minute & ":" & Second)
        PrintLine(1, "")

        FileClose()

        '##### BACKUP #####'
        FileDir = "C:\Record_FCT_N4_K2SA_K2SG_Backup.txt"

        If My.Computer.FileSystem.FileExists(FileDir) Then
        Else
            System.IO.File.Create(FileDir).Dispose()
        End If

        FileOpen(1, FileDir, OpenMode.Append)
        PrintLine(1, CounterOKN4 & "_" & CounterNGN4 & "_" & NGRateN4 & "_" & Year & "-" & Month & "-" & Day & "_" & Hour & ":" & Minute & ":" & Second)
        PrintLine(1, "")

        FileClose()
    End Sub

End Class
