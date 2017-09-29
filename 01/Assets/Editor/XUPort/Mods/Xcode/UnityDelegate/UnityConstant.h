//
//  UnityConstant.h
//  Unity-iPhone
//
//  Created by kang on 2017/9/14.
//
//

#ifndef UnityConstant_h
#define UnityConstant_h

#define Unity_MethodKey @"k"            // the key of Unity object
#define Unity_EnumKey @"e"              // the key of Unity Method
#define Unity_ParamKey @"v"             // the key of Unity param

#define UnityRoot @"Root"

#define UnityFunction @"ResolutionJson" //OpenPara  onResume  LIFECYCLE  WX_NOT_INSTALL ResolutionJson
#define Unity_enum_wxInstalled @"WXInstalled"
#define Untiy_LIFECYCLE @"LIFECYCLE"
#define Untiy_OnPause @"OnPause"
#define Untiy_OnResume @"onResume"
#define Untiy_TempCode @"WXTempCode"
#define Untiy_OpenPara @"OpenPara"
#define Unity_WXPay @"WXPay"

typedef enum : NSUInteger {
    UnityObjectNone,
    UnityObjectRoot,
    UnityObjectOthers,
} UnityObject;


#endif /* UnityConstant_h */
