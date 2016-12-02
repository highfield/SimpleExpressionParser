# SimpleExpressionParser
Simple C#-like expression parser which transforms into a node tree

The library does not require anything special: just C# on any .Net flavor.

## Sample output

Expression:
`    `

Result:
```
Empty expression.
```
***


Expression:
`false  `

Result:
```
<FalseLiteral data="false" />
```
***


Expression:
`  true`

Result:
```
<TrueLiteral data="true" />
```
***


Expression:
`  null  `

Result:
```
<NullLiteral />
```
***


Expression:
` 123 `

Result:
```
<Number data="123" />
```
***


Expression:
`-456  `

Result:
```
<Number data="-456" />
```
***


Expression:
` +7 `

Result:
```
<Number data="7" />
```
***


Expression:
` 3.151927 `

Result:
```
<Number data="3.151927" />
```
***


Expression:
` +2.718 `

Result:
```
<Number data="2.718" />
```
***


Expression:
` .5 `

Result:
```
Illegal character found: .
```
***


Expression:
` -.5 `

Result:
```
Illegal character found: .
```
***


Expression:
` +5. `

Result:
```
Illegal character found:  
```
***


Expression:
`myvar  `

Result:
```
<Identifier data="myvar" />
```
***


Expression:
`  w0rd`

Result:
```
<Identifier data="w0rd" />
```
***


Expression:
`_abc_def_`

Result:
```
<Identifier data="_abc_def_" />
```
***


Expression:
`'single-quoted string'`

Result:
```
<String data="single-quoted string" />
```
***


Expression:
`"double-quoted string"`

Result:
```
<String data="double-quoted string" />
```
***


Expression:
`'here is a "nested string"'`

Result:
```
<String data="here is a &quot;nested string&quot;" />
```
***


Expression:
`zero == zero  `

Result:
```
<OpEqual>
  <Identifier data="zero" />
  <Identifier data="zero" />
</OpEqual>
```
***


Expression:
` black != white`

Result:
```
<OpNotEqual>
  <Identifier data="black" />
  <Identifier data="white" />
</OpNotEqual>
```
***


Expression:
` 12 < 45`

Result:
```
<OpLessThan>
  <Number data="12" />
  <Number data="45" />
</OpLessThan>
```
***


Expression:
`20 >4`

Result:
```
<OpGreaterThan>
  <Number data="20" />
  <Number data="4" />
</OpGreaterThan>
```
***


Expression:
`10<=100`

Result:
```
<OpLessEqualThan>
  <Number data="10" />
  <Number data="100" />
</OpLessEqualThan>
```
***


Expression:
`100   >=   1`

Result:
```
<OpGreaterEqualThan>
  <Number data="100" />
  <Number data="1" />
</OpGreaterEqualThan>
```
***


Expression:
`!false==!!true`

Result:
```
<OpEqual>
  <OpNot>
    <FalseLiteral data="false" />
  </OpNot>
  <OpNot>
    <OpNot>
      <TrueLiteral data="true" />
    </OpNot>
  </OpNot>
</OpEqual>
```
***


Expression:
`to_be || !to_be`

Result:
```
<OpOr>
  <Identifier data="to_be" />
  <OpNot>
    <Identifier data="to_be" />
  </OpNot>
</OpOr>
```
***


Expression:
` maccheroni || spaghetti || rigatoni`

Result:
```
<OpOr>
  <Identifier data="maccheroni" />
  <OpOr>
    <Identifier data="spaghetti" />
    <Identifier data="rigatoni" />
  </OpOr>
</OpOr>
```
***


Expression:
` sex && drug && rock && roll   `

Result:
```
<OpAnd>
  <Identifier data="sex" />
  <OpAnd>
    <Identifier data="drug" />
    <OpAnd>
      <Identifier data="rock" />
      <Identifier data="roll" />
    </OpAnd>
  </OpAnd>
</OpAnd>
```
***


Expression:
`!me || you && !they `

Result:
```
<OpOr>
  <OpNot>
    <Identifier data="me" />
  </OpNot>
  <OpAnd>
    <Identifier data="you" />
    <OpNot>
      <Identifier data="they" />
    </OpNot>
  </OpAnd>
</OpOr>
```
***


Expression:
`a==b && c!=d`

Result:
```
<OpAnd>
  <OpEqual>
    <Identifier data="a" />
    <Identifier data="b" />
  </OpEqual>
  <OpNotEqual>
    <Identifier data="c" />
    <Identifier data="d" />
  </OpNotEqual>
</OpAnd>
```
***


Expression:
`pname match/abc/`

Result:
```
<OpMatch>
  <Identifier data="pname" />
  <MatchParam data="abc" param="" />
</OpMatch>
```
***


Expression:
`pname match /xyz/ig`

Result:
```
<OpMatch>
  <Identifier data="pname" />
  <MatchParam data="xyz" param="ig" />
</OpMatch>
```
***


Expression:
`pname   match /(\w+)\s(\w+)/`

Result:
```
<OpMatch>
  <Identifier data="pname" />
  <MatchParam data="(\w+)\s(\w+)" param="" />
</OpMatch>
```
***


Expression:
`(!me ||you)&&they`

Result:
```
<OpAnd>
  <OpOr>
    <OpNot>
      <Identifier data="me" />
    </OpNot>
    <Identifier data="you" />
  </OpOr>
  <Identifier data="they" />
</OpAnd>
```
***


Expression:
`!(a=='q') && (b!='x')`

Result:
```
<OpAnd>
  <OpNot>
    <OpEqual>
      <Identifier data="a" />
      <String data="q" />
    </OpEqual>
  </OpNot>
  <OpNotEqual>
    <Identifier data="b" />
    <String data="x" />
  </OpNotEqual>
</OpAnd>
```
***


Expression:
`(a || b) && (c || d) || (e && f)`

Result:
```
<OpOr>
  <OpAnd>
    <OpOr>
      <Identifier data="a" />
      <Identifier data="b" />
    </OpOr>
    <OpOr>
      <Identifier data="c" />
      <Identifier data="d" />
    </OpOr>
  </OpAnd>
  <OpAnd>
    <Identifier data="e" />
    <Identifier data="f" />
  </OpAnd>
</OpOr>
```
***


Expression:
`! (a && (b && c || d && e) || (g == h && j))`

Result:
```
<OpNot>
  <OpOr>
    <OpAnd>
      <Identifier data="a" />
      <OpOr>
        <OpAnd>
          <Identifier data="b" />
          <Identifier data="c" />
        </OpAnd>
        <OpAnd>
          <Identifier data="d" />
          <Identifier data="e" />
        </OpAnd>
      </OpOr>
    </OpAnd>
    <OpAnd>
      <OpEqual>
        <Identifier data="g" />
        <Identifier data="h" />
      </OpEqual>
      <Identifier data="j" />
    </OpAnd>
  </OpOr>
</OpNot>
```
***


Expression:
`!! (((a)==b) && ((((c && ((g)))))))`

Result:
```
<OpNot>
  <OpNot>
    <OpAnd>
      <OpEqual>
        <Identifier data="a" />
        <Identifier data="b" />
      </OpEqual>
      <OpAnd>
        <Identifier data="c" />
        <Identifier data="g" />
      </OpAnd>
    </OpAnd>
  </OpNot>
</OpNot>
```
***


