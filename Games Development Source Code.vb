    Public Class FormMainLevel
    Dim SRight As Boolean 'Shooter moving right 
    Dim SLeft As Boolean 'Shooter moving left
    Dim ShooterSpeed As Integer 'How much the shooter moves
    Dim ShotSpeed As Integer 'How much the shot moves
    Dim InvaderSpeed As Integer 'How much the invader moves 
    Dim InvaderDrop As Integer
    Const NumOfInvaders As Integer = 10 'Number of Invaders'
    Dim IRight(NumOfInvaders) As Boolean
    Dim Invaders(NumOfInvaders) As PictureBox
    Dim X As Integer
    Dim ShotDown As Integer
    Dim Paused As Boolean
    Dim Score As Integer = 0



    Private Sub TimerMain_Tick(sender As Object, e As EventArgs) Handles TimerMain.Tick
        MoveShooter()
        FireShot()
        MoveInvader()
        CheckHit()
        CheckGameOver()

    End Sub

    Private Sub FormMain_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Keys.Right Then 'if the right arrow key is pressed
            SRight = True
            SLeft = False
        End If
        If e.KeyValue = Keys.Left Then ' if the left arrow key is pressed 
            SLeft = True
            SRight = False
        End If

        If e.KeyValue = Keys.Space And Shot.Visible = False Then ' if the space is pressed and the shot not been fired
            Shot.Top = Shooter.Top ' shot comes out the shooters top
            Shot.Left = Shooter.Left + (Shooter.Width / 2) - (Shot.Width / 2) 'Shot is centered in the shooter
            Shot.Visible = True

        End If

    End Sub


    Private Sub MoveShooter()
        If SRight = True And Shooter.Left + Shooter.Width < Me.ClientRectangle.Width Then
            Shooter.Left += ShooterSpeed 'Shooter moves to the right 
        End If
        If SLeft = True And Shooter.Left > Me.ClientRectangle.Left Then
            Shooter.Left -= ShooterSpeed 'Shooter moves to the left
        End If

    End Sub

    Private Sub FormMain_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyValue = Keys.Right Then
            SRight = False 'Shooter is not moving to the right 
        End If
        If e.KeyValue = Keys.Left Then
            SLeft = False 'Shooter is not moving to the left 
        End If
    End Sub

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadInvaders()
        LoadSettings()


    End Sub

    Private Sub LoadSettings()
        Paused = False
        ShotSpeed = 10
        ShooterSpeed = 8
        Shot.Visible = False
        For Me.X = 1 To NumOfInvaders
            IRight(X) = True
            Invaders(X).Left = (-50 * X) - (X * 5)
            Invaders(X).Top = 0
            Invaders(X).Visible = True
        Next

        InvaderSpeed = 5
        InvaderDrop = 50
        SRight = False
        SLeft = False
        TimerMain.Enabled = True

    End Sub


    Private Sub FireShot()
        If Shot.Visible = True Then
            Shot.Top -= ShotSpeed ' Shot moves up

        End If

        If Shot.Top + Shot.Height < Me.ClientRectangle.Top Then
            Shot.Visible = False ' Shothits the top of the window and is ready to fire
        End If
    End Sub

    Private Sub MoveInvader()

        For Me.X = 1 To NumOfInvaders

            If IRight(X) = True Then
                Invaders(X).Left += InvaderSpeed ' Moves invaders to the right 
            Else
                Invaders(X).Left -= InvaderSpeed ' Moves invaders to the left 
            End If

            If Invaders(X).Left + Invaders(X).Width > Me.ClientRectangle.Width And IRight(X) Then
                ' Invaders hit right side of the window and drop
                IRight(X) = False
                Invaders(X).Top += InvaderDrop
            End If

            If Invaders(X).Left < Me.ClientRectangle.Left And IRight(X) = False Then
                ' Invaders hit left side of the window and drop 
                IRight(X) = True
                Invaders(X).Top += InvaderDrop
            End If
        Next
    End Sub

    Private Sub CheckGameOver() 
        For Me.X = 1 To NumOfInvaders
            If Invaders(X).Top + Invaders(X).Height >= Shooter.Top And Invaders(X).Visible = True Then
                TimerMain.Enabled = False
                Me.X = NumOfInvaders
                My.Computer.Audio.Play(My.Resources.Lose, AudioPlayMode.Background)
                MsgBox("Game Over - Earth is Destroyed") 'Message Box Appears "Game Over - Earth is Destroyed"'
                PlayAgain()



            End If

            If ShotDown = NumOfInvaders Then
                TimerMain.Enabled = False
                Me.X = NumOfInvaders
                My.Computer.Audio.Play(My.Resources.Win, AudioPlayMode.Background)
                MsgBox("Earth Is Saved") 'Message Box Appears "Earth is saved"'
                PlayNextLevel()


            End If
        Next

    End Sub

    Private Sub CheckHit()
        For Me.X = 1 To NumOfInvaders
            If (Shot.Top + Shot.Height >= Invaders(X).Top) And (Shot.Top <= Invaders(X).Top + Invaders(X).Height) And (Shot.Left + Shot.Width >= Invaders(X).Left) And (Shot.Left <= Invaders(X).Left + Invaders(X).Width) And Shot.Visible = True And Invaders(X).Visible = True Then
                Invaders(X).Visible = False
                My.Computer.Audio.Play(My.Resources.Hit, AudioPlayMode.Background)
                Shot.Visible = False
                ShotDown += 1
                Score += 10
                LabelScore.Text = "Score: " & Score 'Game Score'


            End If
        Next

    End Sub

    Private Sub LoadInvaders()
        For Me.X = 1 To NumOfInvaders
            Invaders(X) = New PictureBox
            Invaders(X).Image = My.Resources.Invader
            Invaders(X).Width = 50
            Invaders(X).Height = 50
            Invaders(X).BackColor = Color.Transparent
            Controls.Add(Invaders(X))

        Next


    End Sub

    Private Sub FormMain_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress 'Press P to Pause game'
        If e.KeyChar = "p" Or e.KeyChar = "p" Then
            If Paused = True Then
                TimerMain.Enabled = True
                Paused = False
            Else
                TimerMain.Enabled = False
                Paused = True
            End If
    End Sub

    Private Sub PlayAgain()
        Dim Result = MsgBox("Play Again?", MsgBoxStyle.YesNo) 'Message Box Appears Play Again Yes or No'

        If Result = MsgBoxResult.Yes Then
            LoadSettings()
        Else
            Me.Close()
        End If

    End Sub

    Private Sub PlayNextLevel()
        Dim Result = MsgBox("Play Next Level", MsgBoxStyle.YesNo) 'Message Box Appears Play Next Level Yes or No'

        If Result = MsgBoxResult.Yes Then
            FormMainLevel2.Show()
        Else
            Me.Close()
        End If

    End Sub
End Class