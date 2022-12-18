using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ComfyJazzSharp;

public sealed class SoundProcessorViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly SoundProcessor _processor;

        private double _tempo;
        private double _pitch;
        private double _rate;
        private string? _status;
        private string? _filename;

        private enum PlaybackMode
        {
            Unloaded,
            Stopped,
            Playing,
            Paused,
        }

        public SoundProcessorViewModel(double? volume = null, double? rate = null, double? pitch = null)
        {
            _processor = new SoundProcessor();
            _processor.PlaybackStopped += OnPlaybackStopped;

            SetPresetValuesIfProvided(volume, rate, pitch);

            Play = new Command(OnPlay);
            Pause = new Command(OnPause);
            Stop = new Command(OnStop);

            SetPlaybackMode(PlaybackMode.Unloaded);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Command Play { get; }

        public Command Pause { get; }

        public Command Stop { get; }

        public string? Status
        {
            get => _status;
            private set => Set(ref _status, value);
        }

        public double Volume { get; set; }

        public double Tempo
        {
            get => _tempo;
            set
            {
                Set(ref _tempo, value);

                if (_processor.ProcessorStream != null)
                    _processor.ProcessorStream.TempoChange = value;
            }
        }

        public double Pitch
        {
            get => _pitch;
            set
            {
                Set(ref _pitch, value);
                if (_processor.ProcessorStream != null)
                    _processor.ProcessorStream.PitchSemiTones = value;
            }
        }

        public double Rate
        {
            get => _rate;
            set
            {
                Set(ref _rate, value);
                if (_processor.ProcessorStream != null)
                    _processor.ProcessorStream.RateChange = value;
            }
        }

        public string? Filename
        {
            get => _filename;
            set => Set(ref _filename, value);
        }

        public void Dispose()
        {
            _processor.Dispose();
        }

        public void OpenFile(string filename)
        {
            OnStop();
            if (_processor.OpenFile(filename))
            {
                Filename = filename;
                SetPlaybackMode(PlaybackMode.Stopped);
            }
            else
            {
                Filename = string.Empty;
                SetPlaybackMode(PlaybackMode.Unloaded);
            }
        }

        private void OnPlay()
        {
            if (_processor.Play((float)Volume))
            {
                SetPlaybackMode(PlaybackMode.Playing);
            }
        }

        private void OnPause()
        {
            if (_processor.Pause())
            {
                SetPlaybackMode(PlaybackMode.Paused);
            }
        }

        private void OnStop()
        {
            if (_processor.Stop())
            {
                SetPlaybackMode(PlaybackMode.Stopped);
            }
        }

        private void SetPlaybackMode(PlaybackMode mode)
        {
            Stop.IsEnabled = mode is PlaybackMode.Playing or PlaybackMode.Paused;
            Play.IsEnabled = mode is PlaybackMode.Stopped or PlaybackMode.Paused;
            Pause.IsEnabled = mode == PlaybackMode.Playing;

            Status = mode switch
            {
                PlaybackMode.Stopped => "Stopped",
                PlaybackMode.Playing => "Playing",
                PlaybackMode.Paused => "Paused",
                _ => string.Empty,
            };
        }

        private void OnPlaybackStopped(object? sender, bool endReached)
        {
            SetPlaybackMode(endReached ? PlaybackMode.Stopped : PlaybackMode.Paused);
        }

        private void Set<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
                return;

            storage = value;

            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        ///
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="rate"></param>
        /// <param name="pitch"></param>
        private void SetPresetValuesIfProvided(double? volume, double? rate, double? pitch)
        {
            if (volume is not null)
                Volume = volume.Value;

            if (rate is not null)
                Rate = rate.Value;

            if (pitch is not null)
                Pitch = pitch.Value;
        }
    }