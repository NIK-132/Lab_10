progrm fulltest;
var a, b: integer
    c, f: real;
    d: boolean;
    badVar: unknownType;
procedure TestProc(var p1 integer; p2: real);
begin
    p1 := 100;
    p2 := 3.14;
end;
procedure (var p3: integer);
begin
    a = 5;
    a := 5.5;
    TestProc(a, c);
    b := a * (5 + + 2);
    TestProc(a);
    f := 132.52;
    TestProc(f, c);
    Test(a);
    c := a + 10.5;
    d := true;
begin
    e := 10;
    a := TestProc;
end.
