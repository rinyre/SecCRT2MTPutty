Imports System.Text
Imports System.IO

Namespace Blowfish.Encryption
    ''' <summary>
    ''' Class that provides blowfish encryption.
    ''' </summary>
    Public Class Blowfish
        Const N As Integer = 16
        Const KEYBYTES As Integer = 8

        Shared _P As UInteger() = {&H243F6A88, &H85A308D3UI, &H13198A2E, &H3707344, &HA4093822UI, &H299F31D0, _
         &H82EFA98, &HEC4E6C89UI, &H452821E6, &H38D01377, &HBE5466CFUI, &H34E90C6C, _
         &HC0AC29B7UI, &HC97C50DDUI, &H3F84D5B5, &HB5470917UI, &H9216D5D9UI, &H8979FB1BUI}
        Shared _S As UInteger(,) = {{&HD1310BA6UI, &H98DFB5ACUI, &H2FFD72DB, &HD01ADFB7UI, &HB8E1AFEDUI, &H6A267E96, _
         &HBA7C9045UI, &HF12C7F99UI, &H24A19947, &HB3916CF7UI, &H801F2E2, &H858EFC16UI, _
         &H636920D8, &H71574E69, &HA458FEA3UI, &HF4933D7EUI, &HD95748F, &H728EB658, _
         &H718BCD58, &H82154AEEUI, &H7B54A41D, &HC25A59B5UI, &H9C30D539UI, &H2AF26013, _
         &HC5D1B023UI, &H286085F0, &HCA417918UI, &HB8DB38EFUI, &H8E79DCB0UI, &H603A180E, _
         &H6C9E0E8B, &HB01E8A3EUI, &HD71577C1UI, &HBD314B27UI, &H78AF2FDA, &H55605C60, _
         &HE65525F3UI, &HAA55AB94UI, &H57489862, &H63E81440, &H55CA396A, &H2AAB10B6, _
         &HB4CC5C34UI, &H1141E8CE, &HA15486AFUI, &H7C72E993, &HB3EE1411UI, &H636FBC2A, _
         &H2BA9C55D, &H741831F6, &HCE5C3E16UI, &H9B87931EUI, &HAFD6BA33UI, &H6C24CF5C, _
         &H7A325381, &H28958677, &H3B8F4898, &H6B4BB9AF, &HC4BFE81BUI, &H66282193, _
         &H61D809CC, &HFB21A991UI, &H487CAC60, &H5DEC8032, &HEF845D5DUI, &HE98575B1UI, _
         &HDC262302UI, &HEB651B88UI, &H23893E81, &HD396ACC5UI, &HF6D6FF3, &H83F44239UI, _
         &H2E0B4482, &HA4842004UI, &H69C8F04A, &H9E1F9B5EUI, &H21C66842, &HF6E96C9AUI, _
         &H670C9C61, &HABD388F0UI, &H6A51A0D2, &HD8542F68UI, &H960FA728UI, &HAB5133A3UI, _
         &H6EEF0B6C, &H137A3BE4, &HBA3BF050UI, &H7EFB2A98, &HA1F1651DUI, &H39AF0176, _
         &H66CA593E, &H82430E88UI, &H8CEE8619UI, &H456F9FB4, &H7D84A5C3, &H3B8B5EBE, _
         &HE06F75D8UI, &H85C12073UI, &H401A449F, &H56C16AA6, &H4ED3AA62, &H363F7706, _
         &H1BFEDF72, &H429B023D, &H37D0D724, &HD00A1248UI, &HDB0FEAD3UI, &H49F1C09B, _
         &H75372C9, &H80991B7BUI, &H25D479D8, &HF6E8DEF7UI, &HE3FE501AUI, &HB6794C3BUI, _
         &H976CE0BDUI, &H4C006BA, &HC1A94FB6UI, &H409F60C4, &H5E5C9EC2, &H196A2463, _
         &H68FB6FAF, &H3E6C53B5, &H1339B2EB, &H3B52EC6F, &H6DFC511F, &H9B30952CUI, _
         &HCC814544UI, &HAF5EBD09UI, &HBEE3D004UI, &HDE334AFDUI, &H660F2807, &H192E4BB3, _
         &HC0CBA857UI, &H45C8740F, &HD20B5F39UI, &HB9D3FBDBUI, &H5579C0BD, &H1A60320A, _
         &HD6A100C6UI, &H402C7279, &H679F25FE, &HFB1FA3CCUI, &H8EA5E9F8UI, &HDB3222F8UI, _
         &H3C7516DF, &HFD616B15UI, &H2F501EC8, &HAD0552ABUI, &H323DB5FA, &HFD238760UI, _
         &H53317B48, &H3E00DF82, &H9E5C57BBUI, &HCA6F8CA0UI, &H1A87562E, &HDF1769DBUI, _
         &HD542A8F6UI, &H287EFFC3, &HAC6732C6UI, &H8C4F5573UI, &H695B27B0, &HBBCA58C8UI, _
         &HE1FFA35DUI, &HB8F011A0UI, &H10FA3D98, &HFD2183B8UI, &H4AFCB56C, &H2DD1D35B, _
         &H9A53E479UI, &HB6F84565UI, &HD28E49BCUI, &H4BFB9790, &HE1DDF2DAUI, &HA4CB7E33UI, _
         &H62FB1341, &HCEE4C6E8UI, &HEF20CADAUI, &H36774C01, &HD07E9EFEUI, &H2BF11FB4, _
         &H95DBDA4DUI, &HAE909198UI, &HEAAD8E71UI, &H6B93D5A0, &HD08ED1D0UI, &HAFC725E0UI, _
         &H8E3C5B2FUI, &H8E7594B7UI, &H8FF6E2FBUI, &HF2122B64UI, &H8888B812UI, &H900DF01CUI, _
         &H4FAD5EA0, &H688FC31C, &HD1CFF191UI, &HB3A8C1ADUI, &H2F2F2218, &HBE0E1777UI, _
         &HEA752DFEUI, &H8B021FA1UI, &HE5A0CC0FUI, &HB56F74E8UI, &H18ACF3D6, &HCE89E299UI, _
         &HB4A84FE0UI, &HFD13E0B7UI, &H7CC43B81, &HD2ADA8D9UI, &H165FA266, &H80957705UI, _
         &H93CC7314UI, &H211A1477, &HE6AD2065UI, &H77B5FA86, &HC75442F5UI, &HFB9D35CFUI, _
         &HEBCDAF0CUI, &H7B3E89A0, &HD6411BD3UI, &HAE1E7E49UI, &H250E2D, &H2071B35E, _
         &H226800BB, &H57B8E0AF, &H2464369B, &HF009B91EUI, &H5563911D, &H59DFA6AA, _
         &H78C14389, &HD95A537FUI, &H207D5BA2, &H2E5B9C5, &H83260376UI, &H6295CFA9, _
         &H11C81968, &H4E734A41, &HB3472DCAUI, &H7B14A94A, &H1B510052, &H9A532915UI, _
         &HD60F573FUI, &HBC9BC6E4UI, &H2B60A476, &H81E67400UI, &H8BA6FB5, &H571BE91F, _
         &HF296EC6BUI, &H2A0DD915, &HB6636521UI, &HE7B9F9B6UI, &HFF34052EUI, &HC5855664UI, _
         &H53B02D5D, &HA99F8FA1UI, &H8BA4799, &H6E85076A}, {&H4B7A70E9, &HB5B32944UI, &HDB75092EUI, &HC4192623UI, &HAD6EA6B0UI, &H49A7DF7D, _
         &H9CEE60B8UI, &H8FEDB266UI, &HECAA8C71UI, &H699A17FF, &H5664526C, &HC2B19EE1UI, _
         &H193602A5, &H75094C29, &HA0591340UI, &HE4183A3EUI, &H3F54989A, &H5B429D65, _
         &H6B8FE4D6, &H99F73FD6UI, &HA1D29C07UI, &HEFE830F5UI, &H4D2D38E6, &HF0255DC1UI, _
         &H4CDD2086, &H8470EB26UI, &H6382E9C6, &H21ECC5E, &H9686B3F, &H3EBAEFC9, _
         &H3C971814, &H6B6A70A1, &H687F3584, &H52A0E286, &HB79C5305UI, &HAA500737UI, _
         &H3E07841C, &H7FDEAE5C, &H8E7D44ECUI, &H5716F2B8, &HB03ADA37UI, &HF0500C0DUI, _
         &HF01C1F04UI, &H200B3FF, &HAE0CF51AUI, &H3CB574B2, &H25837A58, &HDC0921BDUI, _
         &HD19113F9UI, &H7CA92FF6, &H94324773UI, &H22F54701, &H3AE5E581, &H37C2DADC, _
         &HC8B57634UI, &H9AF3DDA7UI, &HA9446146UI, &HFD0030E, &HECC8C73EUI, &HA4751E41UI, _
         &HE238CD99UI, &H3BEA0E2F, &H3280BBA1, &H183EB331, &H4E548B38, &H4F6DB908, _
         &H6F420D03, &HF60A04BFUI, &H2CB81290, &H24977C79, &H5679B072, &HBCAF89AFUI, _
         &HDE9A771FUI, &HD9930810UI, &HB38BAE12UI, &HDCCF3F2EUI, &H5512721F, &H2E6B7124, _
         &H501ADDE6, &H9F84CD87UI, &H7A584718, &H7408DA17, &HBC9F9ABCUI, &HE94B7D8CUI, _
         &HEC7AEC3AUI, &HDB851DFAUI, &H63094366, &HC464C3D2UI, &HEF1C1847UI, &H3215D908, _
         &HDD433B37UI, &H24C2BA16, &H12A14D43, &H2A65C451, &H50940002, &H133AE4DD, _
         &H71DFF89E, &H10314E55, &H81AC77D6UI, &H5F11199B, &H43556F1, &HD7A3C76BUI, _
         &H3C11183B, &H5924A509, &HF28FE6EDUI, &H97F1FBFAUI, &H9EBABF2CUI, &H1E153C6E, _
         &H86E34570UI, &HEAE96FB1UI, &H860E5E0AUI, &H5A3E2AB3, &H771FE71C, &H4E3D06FA, _
         &H2965DCB9, &H99E71D0FUI, &H803E89D6UI, &H5266C825, &H2E4CC978, &H9C10B36AUI, _
         &HC6150EBAUI, &H94E2EA78UI, &HA5FC3C53UI, &H1E0A2DF4, &HF2F74EA7UI, &H361D2B3D, _
         &H1939260F, &H19C27960, &H5223A708, &HF71312B6UI, &HEBADFE6EUI, &HEAC31F66UI, _
         &HE3BC4595UI, &HA67BC883UI, &HB17F37D1UI, &H18CFF28, &HC332DDEFUI, &HBE6C5AA5UI, _
         &H65582185, &H68AB9802, &HEECEA50FUI, &HDB2F953BUI, &H2AEF7DAD, &H5B6E2F84, _
         &H1521B628, &H29076170, &HECDD4775UI, &H619F1510, &H13CCA830, &HEB61BD96UI, _
         &H334FE1E, &HAA0363CFUI, &HB5735C90UI, &H4C70A239, &HD59E9E0BUI, &HCBAADE14UI, _
         &HEECC86BCUI, &H60622CA7, &H9CAB5CABUI, &HB2F3846EUI, &H648B1EAF, &H19BDF0CA, _
         &HA02369B9UI, &H655ABB50, &H40685A32, &H3C2AB4B3, &H319EE9D5, &HC021B8F7UI, _
         &H9B540B19UI, &H875FA099UI, &H95F7997EUI, &H623D7DA8, &HF837889AUI, &H97E32D77UI, _
         &H11ED935F, &H16681281, &HE358829, &HC7E61FD6UI, &H96DEDFA1UI, &H7858BA99, _
         &H57F584A5, &H1B227263, &H9B83C3FFUI, &H1AC24696, &HCDB30AEBUI, &H532E3054, _
         &H8FD948E4UI, &H6DBC3128, &H58EBF2EF, &H34C6FFEA, &HFE28ED61UI, &HEE7C3C73UI, _
         &H5D4A14D9, &HE864B7E3UI, &H42105D14, &H203E13E0, &H45EEE2B6, &HA3AAABEAUI, _
         &HDB6C4F15UI, &HFACB4FD0UI, &HC742F442UI, &HEF6ABBB5UI, &H654F3B1D, &H41CD2105, _
         &HD81E799EUI, &H86854DC7UI, &HE44B476AUI, &H3D816250, &HCF62A1F2UI, &H5B8D2646, _
         &HFC8883A0UI, &HC1C7B6A3UI, &H7F1524C3, &H69CB7492, &H47848A0B, &H5692B285, _
         &H95BBF00, &HAD19489DUI, &H1462B174, &H23820E00, &H58428D2A, &HC55F5EA, _
         &H1DADF43E, &H233F7061, &H3372F092, &H8D937E41UI, &HD65FECF1UI, &H6C223BDB, _
         &H7CDE3759, &HCBEE7460UI, &H4085F2A7, &HCE77326EUI, &HA6078084UI, &H19F8509E, _
         &HE8EFD855UI, &H61D99735, &HA969A7AAUI, &HC50C06C2UI, &H5A04ABFC, &H800BCADCUI, _
         &H9E447A2EUI, &HC3453484UI, &HFDD56705UI, &HE1E9EC9, &HDB73DBD3UI, &H105588CD, _
         &H675FDA79, &HE3674340UI, &HC5C43465UI, &H713E38D8, &H3D28F89E, &HF16DFF20UI, _
         &H153E21E7, &H8FB03D4AUI, &HE6E39F2BUI, &HDB83ADF7UI}, {&HE93D5A68UI, &H948140F7UI, &HF64C261CUI, &H94692934UI, &H411520F7, &H7602D4F7, _
         &HBCF46B2EUI, &HD4A20068UI, &HD4082471UI, &H3320F46A, &H43B7D4B7, &H500061AF, _
         &H1E39F62E, &H97244546UI, &H14214F74, &HBF8B8840UI, &H4D95FC1D, &H96B591AFUI, _
         &H70F4DDD3, &H66A02F45, &HBFBC09ECUI, &H3BD9785, &H7FAC6DD0, &H31CB8504, _
         &H96EB27B3UI, &H55FD3941, &HDA2547E6UI, &HABCA0A9AUI, &H28507825, &H530429F4, _
         &HA2C86DA, &HE9B66DFBUI, &H68DC1462, &HD7486900UI, &H680EC0A4, &H27A18DEE, _
         &H4F3FFEA2, &HE887AD8CUI, &HB58CE006UI, &H7AF4D6B6, &HAACE1E7CUI, &HD3375FECUI, _
         &HCE78A399UI, &H406B2A42, &H20FE9E35, &HD9F385B9UI, &HEE39D7ABUI, &H3B124E8B, _
         &H1DC9FAF7, &H4B6D1856, &H26A36631, &HEAE397B2UI, &H3A6EFA74, &HDD5B4332UI, _
         &H6841E7F7, &HCA7820FBUI, &HFB0AF54EUI, &HD8FEB397UI, &H454056AC, &HBA489527UI, _
         &H55533A3A, &H20838D87, &HFE6BA9B7UI, &HD096954BUI, &H55A867BC, &HA1159A58UI, _
         &HCCA92963UI, &H99E1DB33UI, &HA62A4A56UI, &H3F3125F9, &H5EF47E1C, &H9029317CUI, _
         &HFDF8E802UI, &H4272F70, &H80BB155CUI, &H5282CE3, &H95C11548UI, &HE4C66D22UI, _
         &H48C1133F, &HC70F86DCUI, &H7F9C9EE, &H41041F0F, &H404779A4, &H5D886E17, _
         &H325F51EB, &HD59BC0D1UI, &HF2BCC18FUI, &H41113564, &H257B7834, &H602A9C60, _
         &HDFF8E8A3UI, &H1F636C1B, &HE12B4C2, &H2E1329E, &HAF664FD1UI, &HCAD18115UI, _
         &H6B2395E0, &H333E92E1, &H3B240B62, &HEEBEB922UI, &H85B2A20EUI, &HE6BA0D99UI, _
         &HDE720C8CUI, &H2DA2F728, &HD0127845UI, &H95B794FDUI, &H647D0862, &HE7CCF5F0UI, _
         &H5449A36F, &H877D48FAUI, &HC39DFD27UI, &HF33E8D1EUI, &HA476341, &H992EFF74UI, _
         &H3A6F6EAB, &HF4F8FD37UI, &HA812DC60UI, &HA1EBDDF8UI, &H991BE14CUI, &HDB6E6B0DUI, _
         &HC67B5510UI, &H6D672C37, &H2765D43B, &HDCD0E804UI, &HF1290DC7UI, &HCC00FFA3UI, _
         &HB5390F92UI, &H690FED0B, &H667B9FFB, &HCEDB7D9CUI, &HA091CF0BUI, &HD9155EA3UI, _
         &HBB132F88UI, &H515BAD24, &H7B9479BF, &H763BD6EB, &H37392EB3, &HCC115979UI, _
         &H8026E297UI, &HF42E312DUI, &H6842ADA7, &HC66A2B3BUI, &H12754CCC, &H782EF11C, _
         &H6A124237, &HB79251E7UI, &H6A1BBE6, &H4BFB6350, &H1A6B1018, &H11CAEDFA, _
         &H3D25BDD8, &HE2E1C3C9UI, &H44421659, &HA121386, &HD90CEC6EUI, &HD5ABEA2AUI, _
         &H64AF674E, &HDA86A85FUI, &HBEBFE988UI, &H64E4C3FE, &H9DBC8057UI, &HF0F7C086UI, _
         &H60787BF8, &H6003604D, &HD1FD8346UI, &HF6381FB0UI, &H7745AE04, &HD736FCCCUI, _
         &H83426B33UI, &HF01EAB71UI, &HB0804187UI, &H3C005E5F, &H77A057BE, &HBDE8AE24UI, _
         &H55464299, &HBF582E61UI, &H4E58F48F, &HF2DDFDA2UI, &HF474EF38UI, &H8789BDC2UI, _
         &H5366F9C3, &HC8B38E74UI, &HB475F255UI, &H46FCD9B9, &H7AEB2661, &H8B1DDF84UI, _
         &H846A0E79UI, &H915F95E2UI, &H466E598E, &H20B45770, &H8CD55591UI, &HC902DE4CUI, _
         &HB90BACE1UI, &HBB8205D0UI, &H11A86248, &H7574A99E, &HB77F19B6UI, &HE0A9DC09UI, _
         &H662D09A1, &HC4324633UI, &HE85A1F02UI, &H9F0BE8C, &H4A99A025, &H1D6EFE10, _
         &H1AB93D1D, &HBA5A4DF, &HA186F20FUI, &H2868F169, &HDCB7DA83UI, &H573906FE, _
         &HA1E2CE9BUI, &H4FCD7F52, &H50115E01, &HA70683FAUI, &HA002B5C4UI, &HDE6D027, _
         &H9AF88C27UI, &H773F8641, &HC3604C06UI, &H61A806B5, &HF0177A28UI, &HC0F586E0UI, _
         &H6058AA, &H30DC7D62, &H11E69ED7, &H2338EA63, &H53C2DD94, &HC2C21634UI, _
         &HBBCBEE56UI, &H90BCB6DEUI, &HEBFC7DA1UI, &HCE591D76UI, &H6F05E409, &H4B7C0188, _
         &H39720A3D, &H7C927C24, &H86E3725FUI, &H724D9DB9, &H1AC15BB4, &HD39EB8FCUI, _
         &HED545578UI, &H8FCA5B5, &HD83D7CD3UI, &H4DAD0FC4, &H1E50EF5E, &HB161E6F8UI, _
         &HA28514D9UI, &H6C51133C, &H6FD5C7E7, &H56E14EC4, &H362ABFCE, &HDDC6C837UI, _
         &HD79A3234UI, &H92638212UI, &H670EFA8E, &H406000E0}, {&H3A39CE37, &HD3FAF5CFUI, &HABC27737UI, &H5AC52D1B, &H5CB0679E, &H4FA33742, _
         &HD3822740UI, &H99BC9BBEUI, &HD5118E9DUI, &HBF0F7315UI, &HD62D1C7EUI, &HC700C47BUI, _
         &HB78C1B6BUI, &H21A19045, &HB26EB1BEUI, &H6A366EB4, &H5748AB2F, &HBC946E79UI, _
         &HC6A376D2UI, &H6549C2C8, &H530FF8EE, &H468DDE7D, &HD5730A1DUI, &H4CD04DC6, _
         &H2939BBDB, &HA9BA4650UI, &HAC9526E8UI, &HBE5EE304UI, &HA1FAD5F0UI, &H6A2D519A, _
         &H63EF8CE2, &H9A86EE22UI, &HC089C2B8UI, &H43242EF6, &HA51E03AAUI, &H9CF2D0A4UI, _
         &H83C061BAUI, &H9BE96A4DUI, &H8FE51550UI, &HBA645BD6UI, &H2826A2F9, &HA73A3AE1UI, _
         &H4BA99586, &HEF5562E9UI, &HC72FEFD3UI, &HF752F7DAUI, &H3F046F69, &H77FA0A59, _
         &H80E4A915UI, &H87B08601UI, &H9B09E6ADUI, &H3B3EE593, &HE990FD5AUI, &H9E34D797UI, _
         &H2CF0B7D9, &H22B8B51, &H96D5AC3AUI, &H17DA67D, &HD1CF3ED6UI, &H7C7D2D28, _
         &H1F9F25CF, &HADF2B89BUI, &H5AD6B472, &H5A88F54C, &HE029AC71UI, &HE019A5E6UI, _
         &H47B0ACFD, &HED93FA9BUI, &HE8D3C48DUI, &H283B57CC, &HF8D56629UI, &H79132E28, _
         &H785F0191, &HED756055UI, &HF7960E44UI, &HE3D35E8CUI, &H15056DD4, &H88F46DBAUI, _
         &H3A16125, &H564F0BD, &HC3EB9E15UI, &H3C9057A2, &H97271AECUI, &HA93A072AUI, _
         &H1B3F6D9B, &H1E6321F5, &HF59C66FBUI, &H26DCF319, &H7533D928, &HB155FDF5UI, _
         &H3563482, &H8ABA3CBBUI, &H28517711, &HC20AD9F8UI, &HABCC5167UI, &HCCAD925FUI, _
         &H4DE81751, &H3830DC8E, &H379D5862, &H9320F991UI, &HEA7A90C2UI, &HFB3E7BCEUI, _
         &H5121CE64, &H774FBE32, &HA8B6E37EUI, &HC3293D46UI, &H48DE5369, &H6413E680, _
         &HA2AE0810UI, &HDD6DB224UI, &H69852DFD, &H9072166, &HB39A460AUI, &H6445C0DD, _
         &H586CDECF, &H1C20C8AE, &H5BBEF7DD, &H1B588D40, &HCCD2017FUI, &H6BB4E3BB, _
         &HDDA26A7EUI, &H3A59FF45, &H3E350A44, &HBCB4CDD5UI, &H72EACEA8, &HFA6484BBUI, _
         &H8D6612AEUI, &HBF3C6F47UI, &HD29BE463UI, &H542F5D9E, &HAEC2771BUI, &HF64E6370UI, _
         &H740E0D8D, &HE75B1357UI, &HF8721671UI, &HAF537D5DUI, &H4040CB08, &H4EB4E2CC, _
         &H34D2466A, &H115AF84, &HE1B00428UI, &H95983A1DUI, &H6B89FB4, &HCE6EA048UI, _
         &H6F3F3B82, &H3520AB82, &H11A1D4B, &H277227F8, &H611560B1, &HE7933FDCUI, _
         &HBB3A792BUI, &H344525BD, &HA08839E1UI, &H51CE794B, &H2F32C9B7, &HA01FBAC9UI, _
         &HE01CC87EUI, &HBCC7D1F6UI, &HCF0111C3UI, &HA1E8AAC7UI, &H1A908749, &HD44FBD9AUI, _
         &HD0DADECBUI, &HD50ADA38UI, &H339C32A, &HC6913667UI, &H8DF9317CUI, &HE0B12B4FUI, _
         &HF79E59B7UI, &H43F5BB3A, &HF2D519FFUI, &H27D9459C, &HBF97222CUI, &H15E6FC2A, _
         &HF91FC71, &H9B941525UI, &HFAE59361UI, &HCEB69CEBUI, &HC2A86459UI, &H12BAA8D1, _
         &HB6C1075EUI, &HE3056A0CUI, &H10D25065, &HCB03A442UI, &HE0EC6E0EUI, &H1698DB3B, _
         &H4C98A0BE, &H3278E964, &H9F1F9532UI, &HE0D392DFUI, &HD3A0342BUI, &H8971F21EUI, _
         &H1B0A7441, &H4BA3348C, &HC5BE7120UI, &HC37632D8UI, &HDF359F8DUI, &H9B992F2EUI, _
         &HE60B6F47UI, &HFE3F11D, &HE54CDA54UI, &H1EDAD891, &HCE6279CFUI, &HCD3E7E6FUI, _
         &H1618B166, &HFD2C1D05UI, &H848FD2C5UI, &HF6FB2299UI, &HF523F357UI, &HA6327623UI, _
         &H93A83531UI, &H56CCCD02, &HACF08162UI, &H5A75EBB5, &H6E163697, &H88D273CCUI, _
         &HDE966292UI, &H81B949D0UI, &H4C50901B, &H71C65614, &HE6C6C7BDUI, &H327A140A, _
         &H45E1D006, &HC3F27B9AUI, &HC9AA53FDUI, &H62A80F00, &HBB25BFE2UI, &H35BDD2F6, _
         &H71126905, &HB2040222UI, &HB6CBCF7CUI, &HCD769C2BUI, &H53113EC0, &H1640E3D3, _
         &H38ABBD60, &H2547ADF0, &HBA38209CUI, &HF746CE76UI, &H77AFA1C5, &H20756060, _
         &H85CBFE4EUI, &H8AE88DD8UI, &H7AAAF9B0, &H4CF9AA7E, &H1948C25C, &H2FB8A8C, _
         &H1C36AE4, &HD6EBE1F9UI, &H90D4F869UI, &HA65CDEA0UI, &H3F09252D, &HC208E69FUI, _
         &HB74E6132UI, &HCE77E25BUI, &H578FDFE3, &H3AC372E6}}

        Private P As UInteger()
        Private S As UInteger(,)

        ''' <summary>
        ''' Constructs and initializes a blowfish instance with the supplied key.
        ''' </summary>
        ''' <param name="key">The key to cipher with.</param>
        Public Sub New(key As Byte())
            Dim i As Short
            Dim j As Short
            Dim k As Short
            Dim data As UInteger
            Dim datal As UInteger
            Dim datar As UInteger

            P = TryCast(_P.Clone(), UInteger())
            S = TryCast(_S.Clone(), UInteger(,))

            j = 0
            For i = 0 To N + 1
                data = &H0
                For k = 0 To 3
                    data = (data << 8) Or key(j)
                    j += 1
                    If j >= key.Length Then
                        j = 0
                    End If
                Next
                P(i) = P(i) Xor data
            Next

            datal = &H0
            datar = &H0

            For i = 0 To N + 1 Step 2
                Encipher(datal, datar)
                P(i) = datal
                P(i + 1) = datar
            Next

            For i = 0 To 3
                For j = 0 To 255 Step 2
                    Encipher(datal, datar)

                    S(i, j) = datal
                    S(i, j + 1) = datar
                Next
            Next
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="x"></param>
        ''' <returns></returns>
        Private Function F(x As UInteger) As UInteger
            Dim a As UShort
            Dim b As UShort
            Dim c As UShort
            Dim d As UShort
            Dim y As UInteger

            d = CUShort(x And &HFF)
            x >>= 8
            c = CUShort(x And &HFF)
            x >>= 8
            b = CUShort(x And &HFF)
            x >>= 8
            a = CUShort(x And &HFF)
            'y = ((S[0][a] + S[1][b]) ^ S[2][c]) + S[3][d];
            y = S(0, a) + S(1, b)
            y = y Xor S(2, c)
            y = y + S(3, d)

            Return y
        End Function

        ''' <summary>
        ''' Encrypts a byte array in place.
        ''' </summary>
        ''' <param name="data">The array to encrypt.</param>
        ''' <param name="length">The amount to encrypt.</param>
        Public Sub Encipher(data As Byte(), length As Integer)
            Dim xl As UInteger, xr As UInteger
            If (length Mod 8) <> 0 Then
                Throw New Exception("Invalid Length")
            End If
            For i As Integer = 0 To length - 1 Step 8
                ' Encode the data in 8 byte blocks.
                xl = CUInt((data(i) << 24) Or (data(i + 1) << 16) Or (data(i + 2) << 8) Or data(i + 3))
                xr = CUInt((data(i + 4) << 24) Or (data(i + 5) << 16) Or (data(i + 6) << 8) Or data(i + 7))
                Encipher(xl, xr)
                ' Now Replace the data.
                data(i) = CByte(xl >> 24)
                data(i + 1) = CByte(xl >> 16)
                data(i + 2) = CByte(xl >> 8)
                data(i + 3) = CByte(xl)
                data(i + 4) = CByte(xr >> 24)
                data(i + 5) = CByte(xr >> 16)
                data(i + 6) = CByte(xr >> 8)
                data(i + 7) = CByte(xr)
            Next
        End Sub

        ''' <summary>
        ''' Encrypts 8 bytes of data (1 block)
        ''' </summary>
        ''' <param name="xl">The left part of the 8 bytes.</param>
        ''' <param name="xr">The right part of the 8 bytes.</param>
        Private Sub Encipher(ByRef xl__1 As UInteger, ByRef xr__2 As UInteger)
            Dim Xl__3 As UInteger
            Dim Xr__4 As UInteger
            Dim temp As UInteger
            Dim i As Short

            Xl__3 = xl__1
            Xr__4 = xr__2

            For i = 0 To N - 1
                Xl__3 = Xl__3 Xor P(i)
                Xr__4 = F(Xl__3) Xor Xr__4

                temp = Xl__3
                Xl__3 = Xr__4
                Xr__4 = temp
            Next

            temp = Xl__3
            Xl__3 = Xr__4
            Xr__4 = temp

            Xr__4 = Xr__4 Xor P(N)
            Xl__3 = Xl__3 Xor P(N + 1)

            xl__1 = Xl__3
            xr__2 = Xr__4
        End Sub

        ''' <summary>
        ''' Decrypts a byte array in place.
        ''' </summary>
        ''' <param name="data">The array to decrypt.</param>
        ''' <param name="length">The amount to decrypt.</param>
        Public Sub Decipher(data As Byte(), length As Integer)
            Dim xl As UInteger, xr As UInteger
            If (length Mod 8) <> 0 Then
                Throw New Exception("Invalid Length")
            End If
            For i As Integer = 0 To length - 1 Step 8
                ' Encode the data in 8 byte blocks.
                xl = CUInt((data(i) << 24) Or (data(i + 1) << 16) Or (data(i + 2) << 8) Or data(i + 3))
                xr = CUInt((data(i + 4) << 24) Or (data(i + 5) << 16) Or (data(i + 6) << 8) Or data(i + 7))
                Decipher(xl, xr)
                ' Now Replace the data.
                data(i) = CByte(xl >> 24)
                data(i + 1) = CByte(xl >> 16)
                data(i + 2) = CByte(xl >> 8)
                data(i + 3) = CByte(xl)
                data(i + 4) = CByte(xr >> 24)
                data(i + 5) = CByte(xr >> 16)
                data(i + 6) = CByte(xr >> 8)
                data(i + 7) = CByte(xr)
            Next
        End Sub

        ''' <summary>
        ''' Decrypts 8 bytes of data (1 block)
        ''' </summary>
        ''' <param name="xl">The left part of the 8 bytes.</param>
        ''' <param name="xr">The right part of the 8 bytes.</param>
        Private Sub Decipher(ByRef xl__1 As UInteger, ByRef xr__2 As UInteger)
            Dim Xl__3 As UInteger
            Dim Xr__4 As UInteger
            Dim temp As UInteger
            Dim i As Short

            Xl__3 = xl__1
            Xr__4 = xr__2

            For i = N + 1 To 2 Step -1
                Xl__3 = Xl__3 Xor P(i)
                Xr__4 = F(Xl__3) Xor Xr__4

                ' Exchange Xl and Xr 

                temp = Xl__3
                Xl__3 = Xr__4
                Xr__4 = temp
            Next

            ' Exchange Xl and Xr 

            temp = Xl__3
            Xl__3 = Xr__4
            Xr__4 = temp

            Xr__4 = Xr__4 Xor P(1)
            Xl__3 = Xl__3 Xor P(0)

            xl__1 = Xl__3
            xr__2 = Xr__4
        End Sub
    End Class

    Public Class BlowfishStream
        Inherits Stream
        Private Class CBState
            Implements IAsyncResult
            Friend callback As AsyncCallback
            Friend state As Object
            Friend buffer As Byte()
            Friend result As IAsyncResult
            Friend Sub New(callback As AsyncCallback, state As Object, buffer As Byte())
                Me.callback = callback
                Me.state = state
                Me.buffer = buffer
            End Sub
#Region "IAsyncResult Members"

            Public ReadOnly Property AsyncState() As Object Implements IAsyncResult.AsyncState
                Get
                    Return state
                End Get
            End Property

            Public ReadOnly Property CompletedSynchronously() As Boolean Implements IAsyncResult.CompletedSynchronously
                Get
                    Return result.CompletedSynchronously
                End Get
            End Property

            Public ReadOnly Property AsyncWaitHandle() As System.Threading.WaitHandle Implements IAsyncResult.AsyncWaitHandle
                Get
                    Return result.AsyncWaitHandle
                End Get
            End Property

            Public ReadOnly Property IsCompleted() As Boolean Implements IAsyncResult.IsCompleted
                Get
                    Return result.IsCompleted
                End Get
            End Property

#End Region
        End Class

        Public Enum Target
            Encrypted
            Normal
        End Enum

        Private stream As Stream
        Private bf As Blowfish
        Private target1 As Target

        Private Sub New(stream As Stream, bf As Blowfish, target As Target)
            Me.stream = stream
            Me.bf = bf
            Me.target1 = target
        End Sub

        ''' <summary>
        ''' Returns true if the stream support reads.
        ''' </summary>
        Public Overrides ReadOnly Property CanRead() As Boolean
            Get
                Return stream.CanRead
            End Get
        End Property

        ''' <summary>
        ''' Returns true is the stream supports seeks.
        ''' </summary>
        Public Overrides ReadOnly Property CanSeek() As Boolean
            Get
                Return stream.CanSeek
            End Get
        End Property

        ''' <summary>
        ''' Returns true if the stream supports writes.
        ''' </summary>
        Public Overrides ReadOnly Property CanWrite() As Boolean
            Get
                Return stream.CanWrite
            End Get
        End Property

        ''' <summary>
        ''' Returns the length of the stream.
        ''' </summary>
        Public Overrides ReadOnly Property Length() As Long
            Get
                Return stream.Length
            End Get
        End Property

        ''' <summary>
        ''' Gets or Sets the posistion of the stream.
        ''' </summary>
        Public Overrides Property Position() As Long
            Get
                Return stream.Position
            End Get
            Set(value As Long)
                stream.Position = value
            End Set
        End Property

        ''' <summary>
        ''' Flushes the stream.
        ''' </summary>
        Public Overrides Sub Flush()
            stream.Flush()
        End Sub

        ''' <summary>
        ''' Read data from the stream and encrypt it.
        ''' </summary>
        ''' <param name="buffer">The buffer to read into.</param>
        ''' <param name="offset">The offset in the buffer to begin storing data.</param>
        ''' <param name="count">The number of bytes to read.</param>
        ''' <returns></returns>
        Public Overrides Function Read(buffer As Byte(), offset As Integer, count As Integer) As Integer
            Dim bytesRead As Integer = stream.Read(buffer, offset, count)
            If target1 = Target.Normal Then
                bf.Encipher(buffer, bytesRead)
            Else
                bf.Decipher(buffer, bytesRead)
            End If
            Return bytesRead
        End Function

        ''' <summary>
        ''' Write data to the stream after decrypting it.
        ''' </summary>
        ''' <param name="buffer">The buffer containing the data to write.</param>
        ''' <param name="offset">The offset in the buffer where the data begins.</param>
        ''' <param name="count">The number of bytes to write.</param>
        Public Overrides Sub Write(buffer As Byte(), offset As Integer, count As Integer)
            If target1 = Target.Normal Then
                bf.Decipher(buffer, count)
            Else
                bf.Encipher(buffer, count)
            End If
            stream.Write(buffer, offset, count)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="buffer"></param>
        ''' <param name="offset"></param>
        ''' <param name="count"></param>
        ''' <param name="callback"></param>
        ''' <param name="state"></param>
        ''' <returns></returns>
        Public Overrides Function BeginRead(buffer As Byte(), offset As Integer, count As Integer, callback As AsyncCallback, state As Object) As IAsyncResult
            Dim cbs As New CBState(callback, state, buffer)
            cbs.result = MyBase.BeginRead(buffer, offset, count, New AsyncCallback(AddressOf ReadComplete), cbs)
            Return cbs
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="asyncResult"></param>
        ''' <returns></returns>
        Public Overrides Function EndRead(asyncResult As IAsyncResult) As Integer
            Dim cbs As CBState = DirectCast(asyncResult.AsyncState, CBState)
            Dim bytesRead As Integer = MyBase.EndRead(cbs.result)
            If target1 = Target.Normal Then
                bf.Encipher(cbs.buffer, bytesRead)
            Else
                bf.Decipher(cbs.buffer, bytesRead)
            End If
            Return bytesRead
        End Function


        ''' <summary>
        ''' The Read has completed.
        ''' </summary>
        ''' <param name="result">The result of the async write.</param>
        Private Sub ReadComplete(result As IAsyncResult)
            Dim cbs As CBState = DirectCast(result.AsyncState, CBState)
            cbs.callback(cbs)
        End Sub


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="buffer"></param>
        ''' <param name="offset"></param>
        ''' <param name="count"></param>
        ''' <param name="callback"></param>
        ''' <param name="state"></param>
        ''' <returns></returns>
        Public Overrides Function BeginWrite(buffer As Byte(), offset As Integer, count As Integer, callback As AsyncCallback, state As Object) As IAsyncResult
            If target1 = Target.Normal Then
                bf.Decipher(buffer, count)
            Else
                bf.Encipher(buffer, count)
            End If
            Return MyBase.BeginWrite(buffer, offset, count, callback, state)
        End Function

        ''' <summary>
        ''' Move the current stream posistion to the specified location.
        ''' </summary>
        ''' <param name="offset">The offset from the origin to seek.</param>
        ''' <param name="origin">The origin to seek from.</param>
        ''' <returns>The new position.</returns>
        Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long
            Return stream.Seek(offset, origin)
        End Function

        ''' <summary>
        ''' Set the stream length.
        ''' </summary>
        ''' <param name="value">The length to set.</param>
        Public Overrides Sub SetLength(value As Long)
            stream.SetLength(value)
        End Sub
    End Class
End Namespace