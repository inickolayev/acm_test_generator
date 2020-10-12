﻿using System;
using System.Collections.Generic;

namespace TestGenerator
{
    public class TestBuilder
    {
        private readonly Func<Test, TestSetBuilder> _onBuild;
        private readonly List<TestLine> _testLines = new List<TestLine>();

        private TestBuilder _forBuilder;
        private int _forStart;
        private int _forEnd;

        public TestBuilder(Func<Test, TestSetBuilder> onBuild)
        {
            _onBuild = onBuild;
        }
        public TestSetBuilder BuildTest()
            => _onBuild(new Test(_testLines));

        public TestBuilder AddNumbers(params int[] nums)
        {
            _testLines.Add(new TestLine(string.Join(' ', nums)));
            return this;
        }

        public TestBuilder GenerateNumber(long minValue = long.MinValue, long maxValue = long.MaxValue)
            => GenerateNumbers(1, minValue, maxValue);

        public TestBuilder GenerateNumbers(int size, long minValue = long.MinValue, long maxValue = long.MaxValue)
        {
            _testLines.Add(new TestLine(
                string.Join(' ', GeneratorHelper.GenerateArray(size, minValue, maxValue)))
            );
            return this;
        }

        public TestBuilder For(int start, int end, Func<int, TestBuilder, TestBuilder> generateTest)
        {
            for (int i = start; i <= end; i++)
                generateTest(i, this);
            return this;
        }

        public TestBuilder EndFor()
        {
            for(int i = _forStart; i <= _forEnd; i++)
                _testLines.AddRange(_forBuilder._testLines);
            return this;
        }
    }
}
