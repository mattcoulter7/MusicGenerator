﻿using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Linq;

namespace Melanchall.DryWetMidi.Tools
{
    public static partial class Splitter
    {
        /// <summary>
        /// Skips part of the specified length of MIDI file and returns remaining part as
        /// an instance of <see cref="MidiFile"/>.
        /// </summary>
        /// <param name="midiFile"><see cref="MidiFile"/> to skip part of.</param>
        /// <param name="partLength">The length of part to skip.</param>
        /// <param name="settings">Settings according to which <paramref name="midiFile"/>
        /// should be split.</param>
        /// <returns><see cref="MidiFile"/> which is result of skipping a part of the <paramref name="midiFile"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para>One of the following errors occured:</para>
        /// <list type="bullet">
        /// <item>
        /// <description><paramref name="midiFile"/> is <c>null</c>.</description>
        /// </item>
        /// <item>
        /// <description><paramref name="partLength"/> is <c>null</c>.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <example>
        /// <para>
        /// Given the MIDI file (vertical line shows where the file will be split):
        /// </para>
        /// <code language="image">
        ///  │←─── L ────→│
        /// +-------------║-----------+
        /// |┌────────────║──────────┐|
        /// |│  A    B    ║     C    │|
        /// |└────────────║─────⁞────┘|
        /// |┌────────────║─────⁞────┐|
        /// |│         D  ║  E  ⁞    │|
        /// |└────────────║──⁞──⁞────┘|
        /// +-------------║--⁞--⁞-----+
        /// </code>
        /// <para>
        /// where <c>A</c>, <c>B</c>, <c>C</c>, <c>D</c> and <c>E</c> are some MIDI events;
        /// <c>L</c> is <paramref name="partLength"/>.
        /// </para>
        /// <para>
        /// Skipping the part we'll get following file:
        /// </para>
        /// <code language="image">
        ///              +---⁞--⁞-----+
        ///              |┌──⁞──⁞────┐|
        ///              |│  ⁞  C    │|
        ///              |└──⁞───────┘|
        ///              |┌──⁞───────┐|
        ///              |│  E       │|
        ///              |└──────────┘|
        ///              +------------+
        /// </code>
        /// </example>
        public static MidiFile SkipPart(this MidiFile midiFile, ITimeSpan partLength, SliceMidiFileSettings settings = null)
        {
            ThrowIfArgument.IsNull(nameof(midiFile), midiFile);
            ThrowIfArgument.IsNull(nameof(partLength), partLength);

            var grid = new ArbitraryGrid(partLength);

            settings = settings ?? new SliceMidiFileSettings();
            midiFile = PrepareMidiFileForSlicing(midiFile, grid, settings);

            var tempoMap = midiFile.GetTempoMap();
            var time = grid.GetTimes(tempoMap).First();

            using (var slicer = MidiFileSlicer.CreateFromFile(midiFile))
            {
                slicer.GetNextSlice(time, settings);
                return slicer.GetNextSlice(long.MaxValue, settings);
            }
        }
    }
}
