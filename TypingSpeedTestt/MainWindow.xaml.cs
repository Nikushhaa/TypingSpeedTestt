using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TypingSpeedTestWPF
{
    public partial class MainWindow : Window
    {
        private List<string> sentences = new List<string>
        {
            "The quick brown fox jumps over the lazy dog.",
            "I love programming in C Sharp WPF.",
            "Typing speed tests are fun and challenging.",
            "Visual Studio makes GUI design easy and fast.",
            "Neon colors give a cool futuristic effect.",
            "Consistency is key to improving typing speed.",
            "Practice makes perfect, never give up.",
            "Short sentences are easier to type quickly.",
            "Keep your hands on the home row keys.",
            "Accuracy matters more than just speed.",
            "This is a sample sentence for testing.",
            "Make every keystroke count in this test.",
            "Focus on typing correctly, not just fast.",
            "Learn new words while practicing typing.",
            "WPF allows creating modern desktop applications.",
            "Art exhibitions showcase talent from around the world.",
            "Computers have transformed modern life completely.",
"Gaming improves reflexes and strategic thinking.",
"Watching movies is a fun way to relax.",
"Music can change your mood instantly.",
"Popular artists often inspire millions worldwide.",
"Laptops are convenient for work and study.",
"Playing multiplayer games requires teamwork.",
"Streaming movies online is very popular today.",
"Headphones give an immersive music experience.",
"Digital art is becoming more mainstream.",
"Video game graphics are more realistic now.",
"Some movies are based on true stories.",
"Music festivals attract huge crowds.",
"Programmers write code to solve problems.",
"Console gaming has a loyal fanbase.",
"Movie soundtracks can be unforgettable.",
"Artists experiment with new styles constantly.",
"Desktop computers are still very powerful.",
"Speedrunning games is a challenging hobby.",
"Movie theaters offer a big-screen experience.",
"DJs mix songs for live audiences.",
"Coding requires patience and creativity.",
"Online gaming connects people worldwide.",
"Blockbuster movies break box office records.",
"Musicians release albums for fans regularly.",
"Graphics cards improve gaming performance.",
"Indie games often have unique gameplay.",
"Movie directors create compelling stories.",
"Classical music can be very relaxing.",
"Artists use digital tools to create art.",
"Computers need regular software updates.",
"Streaming platforms offer endless content.",
"Gaming keyboards have colorful backlights.",
"Movie critics share their opinions online.",
"Music streaming services are convenient.",
"Virtual reality gaming feels very real.",
"Actors prepare extensively for their roles.",
"Guitarists practice daily to improve skills.",
"Laptops can be lightweight and portable.",
"Some movies are part of a long series.",
"Music can be both calming and energizing.",
"Developers debug code to fix errors.",
"PC gaming often allows more customization.",
"Movie plots can be predictable or surprising.",
"Artists sell prints of their work online.",
"Computers store massive amounts of data.",
"Competitive gaming requires fast reactions.",
"Animated movies appeal to all ages.",
"Live concerts create unforgettable experiences.",
"Digital art programs are powerful tools.",
"Programming languages are fun to learn.",
"Some video games are story-driven adventures.",
"Film editing shapes the final movie experience.",
"Musicians collaborate to produce new songs.",
"Laptop batteries last for several hours.",
"Esports tournaments attract global viewers.",
"Movie sound effects make scenes more realistic.",
"Artists often share their process online.",
"Cloud storage keeps files safe and accessible.",
"Gaming monitors can have high refresh rates.",
"Movies often teach lessons or morals.",
"Music albums can tell a story through songs.",
"Computer networks connect devices together.",
"Some games require strategic thinking and planning.",
"Film scores enhance the emotional impact.",
"Singers practice vocal techniques every day.",
"Desktop setups can be simple or elaborate.",
"Virtual concerts are becoming more common.",
"Gaming headsets improve sound quality.",
"Movie trailers create excitement before release.",
"Artists use brushes, pencils, or tablets.",
"Programming projects improve problem-solving skills.",
"Retro games still have a loyal fanbase.",
"Movie genres include comedy, action, and horror.",
"Music can connect people across cultures.",
"Computer viruses can damage important files.",
"Gamers often share tips and strategies online.",
"Animated series can be just as popular as movies.",
"Musicians experiment with different instruments.",
"Laptop cooling pads prevent overheating.",
"Some games are based on real historical events.",
"Movie studios release films worldwide simultaneously.",
"Artists often inspire others with their creativity.",
"Programming logic is essential for good software.",
"Board games can be a fun offline activity.",
"Film directors choose locations carefully.",
"Music lessons help improve technical skills.",
"Computer storage can be expanded with external drives.",
"Gamers enjoy both single-player and multiplayer modes.",
"Movies can evoke laughter, tears, or suspense.",
"Artists sometimes collaborate on large projects.",
"Coding efficiently saves time and resources.",
"VR headsets make games more immersive.",
"Movie marathons are fun on weekends.",
"Music playlists can match your daily mood.",
"Computer graphics are used in movies and games.",
"Speed typing improves with consistent practice.",
"Streaming live performances reaches global audiences.",
"Gamers often customize avatars and characters.",
"Art exhibitions showcase talent from around the world.",

        };

        private int currentIndex = 0;
        private int sentenceCount = 5;
        private Stopwatch stopwatch = new Stopwatch();
        private double bestWPM = 0;
        private string bestScoreFile = "bestscore.txt";
        private SolidColorBrush originalBackground;

        public MainWindow()
        {
            InitializeComponent();
            LoadBestScore();
            originalBackground = (SolidColorBrush)this.Background;
        }

        private void LoadBestScore()
        {
            if (File.Exists(bestScoreFile))
            {
                string text = File.ReadAllText(bestScoreFile);
                double.TryParse(text, out bestWPM);
                BestLabel.Text = $"Best WPM: {bestWPM:F2}";
            }
        }

        private void SaveBestScore()
        {
            File.WriteAllText(bestScoreFile, bestWPM.ToString("F2"));
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            currentIndex = 0;
            TypingTextBox.Text = "";
            WPMLabel.Text = "WPM: 0";
            AccuracyLabel.Text = "Accuracy: 0%";

            this.Background = originalBackground;

            sentenceCount = int.Parse(((System.Windows.Controls.ComboBoxItem)SentenceCountComboBox.SelectedItem).Content.ToString());
            SentenceTextBlock.Text = sentences[currentIndex];

            NextButton.IsEnabled = true;
            stopwatch.Restart();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            string typedText = TypingTextBox.Text;
            string currentSentence = sentences[currentIndex];

            // Calculate accuracy
            double correctChars = 0;
            for (int i = 0; i < Math.Min(typedText.Length, currentSentence.Length); i++)
            {
                if (typedText[i] == currentSentence[i])
                    correctChars++;
            }
            double accuracy = (correctChars / currentSentence.Length) * 100;
            AccuracyLabel.Text = $"Accuracy: {accuracy:F2}%";

            // Flash background based on accuracy
            FlashBackground(accuracy);

            // Calculate WPM
            stopwatch.Stop();
            double minutes = stopwatch.Elapsed.TotalSeconds / 60.0;
            int wordCount = typedText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
            double wpm = wordCount / minutes;
            WPMLabel.Text = $"WPM: {wpm:F2}";

            if (wpm > bestWPM)
            {
                bestWPM = wpm;
                BestLabel.Text = $"Best WPM: {bestWPM:F2}";
                SaveBestScore();
            }

            currentIndex++;
            if (currentIndex < sentenceCount)
            {
                TypingTextBox.Text = "";
                SentenceTextBlock.Text = sentences[currentIndex];
                stopwatch.Restart();
            }
            else
            {
                MessageBox.Show("Test Finished!", "Typing Speed Test");
                NextButton.IsEnabled = false;
                SentenceTextBlock.Text = "Click Start to try again!";
            }
        }

        private void FlashBackground(double accuracy)
        {
            Color glowColor;

            if (accuracy >= 90)
                glowColor = Colors.LimeGreen;
            else if (accuracy >= 50)
                glowColor = Colors.Orange;
            else
                glowColor = Colors.Red;

            ColorAnimation animation = new ColorAnimation
            {
                From = glowColor,
                To = ((SolidColorBrush)originalBackground).Color,
                Duration = TimeSpan.FromSeconds(1.5)
            };

            SolidColorBrush brush = new SolidColorBrush(((SolidColorBrush)originalBackground).Color);
            this.Background = brush;
            brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
    }
}
