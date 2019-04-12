using Mocky;
using System;

class MockICalculator : ICalculator {

    private MockMethod[] obj;
    public MockICalculator(MockMethod[] obj) {
        this.obj = obj;
    }

    public int Add(int a, int b) {
        throw new NotImplementedException();
    }

    public int Div(int a, int b) {
        throw new NotImplementedException();
    }

    public int Mul(int a, int b) {
        throw new NotImplementedException();
    }

    public int Sub(int a, int b) {
        throw new NotImplementedException();
    }
}
