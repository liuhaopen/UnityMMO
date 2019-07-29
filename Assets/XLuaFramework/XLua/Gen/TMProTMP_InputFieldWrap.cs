#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class TMProTMP_InputFieldWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(TMPro.TMP_InputField);
			Utils.BeginObjectRegister(type, L, translator, 0, 28, 55, 45);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTextWithoutNotify", _m_SetTextWithoutNotify);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveTextEnd", _m_MoveTextEnd);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveTextStart", _m_MoveTextStart);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveToEndOfLine", _m_MoveToEndOfLine);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveToStartOfLine", _m_MoveToStartOfLine);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBeginDrag", _m_OnBeginDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDrag", _m_OnDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEndDrag", _m_OnEndDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPointerDown", _m_OnPointerDown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ProcessEvent", _m_ProcessEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnUpdateSelected", _m_OnUpdateSelected);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnScroll", _m_OnScroll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ForceLabelUpdate", _m_ForceLabelUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Rebuild", _m_Rebuild);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LayoutComplete", _m_LayoutComplete);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GraphicUpdateComplete", _m_GraphicUpdateComplete);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ActivateInputField", _m_ActivateInputField);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnSelect", _m_OnSelect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPointerClick", _m_OnPointerClick);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnControlClick", _m_OnControlClick);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReleaseSelection", _m_ReleaseSelection);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DeactivateInputField", _m_DeactivateInputField);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDeselect", _m_OnDeselect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnSubmit", _m_OnSubmit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CalculateLayoutInputHorizontal", _m_CalculateLayoutInputHorizontal);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CalculateLayoutInputVertical", _m_CalculateLayoutInputVertical);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetGlobalPointSize", _m_SetGlobalPointSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetGlobalFontAsset", _m_SetGlobalFontAsset);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "shouldHideMobileInput", _g_get_shouldHideMobileInput);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "shouldHideSoftKeyboard", _g_get_shouldHideSoftKeyboard);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "text", _g_get_text);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isFocused", _g_get_isFocused);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "caretBlinkRate", _g_get_caretBlinkRate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "caretWidth", _g_get_caretWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "textViewport", _g_get_textViewport);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "textComponent", _g_get_textComponent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "placeholder", _g_get_placeholder);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "verticalScrollbar", _g_get_verticalScrollbar);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "scrollSensitivity", _g_get_scrollSensitivity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "caretColor", _g_get_caretColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "customCaretColor", _g_get_customCaretColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "selectionColor", _g_get_selectionColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onEndEdit", _g_get_onEndEdit);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onSubmit", _g_get_onSubmit);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onSelect", _g_get_onSelect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onDeselect", _g_get_onDeselect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onTextSelection", _g_get_onTextSelection);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onEndTextSelection", _g_get_onEndTextSelection);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onValueChanged", _g_get_onValueChanged);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onTouchScreenKeyboardStatusChanged", _g_get_onTouchScreenKeyboardStatusChanged);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onValidateInput", _g_get_onValidateInput);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "characterLimit", _g_get_characterLimit);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pointSize", _g_get_pointSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "fontAsset", _g_get_fontAsset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onFocusSelectAll", _g_get_onFocusSelectAll);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "resetOnDeActivation", _g_get_resetOnDeActivation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "restoreOriginalTextOnEscape", _g_get_restoreOriginalTextOnEscape);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isRichTextEditingAllowed", _g_get_isRichTextEditingAllowed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "contentType", _g_get_contentType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "lineType", _g_get_lineType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "lineLimit", _g_get_lineLimit);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "inputType", _g_get_inputType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "keyboardType", _g_get_keyboardType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "characterValidation", _g_get_characterValidation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "inputValidator", _g_get_inputValidator);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "readOnly", _g_get_readOnly);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "richText", _g_get_richText);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "multiLine", _g_get_multiLine);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "asteriskChar", _g_get_asteriskChar);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "wasCanceled", _g_get_wasCanceled);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "caretPosition", _g_get_caretPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "selectionAnchorPosition", _g_get_selectionAnchorPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "selectionFocusPosition", _g_get_selectionFocusPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stringPosition", _g_get_stringPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "selectionStringAnchorPosition", _g_get_selectionStringAnchorPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "selectionStringFocusPosition", _g_get_selectionStringFocusPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "minWidth", _g_get_minWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "preferredWidth", _g_get_preferredWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "flexibleWidth", _g_get_flexibleWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "minHeight", _g_get_minHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "preferredHeight", _g_get_preferredHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "flexibleHeight", _g_get_flexibleHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "layoutPriority", _g_get_layoutPriority);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "shouldHideMobileInput", _s_set_shouldHideMobileInput);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "shouldHideSoftKeyboard", _s_set_shouldHideSoftKeyboard);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "text", _s_set_text);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "caretBlinkRate", _s_set_caretBlinkRate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "caretWidth", _s_set_caretWidth);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "textViewport", _s_set_textViewport);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "textComponent", _s_set_textComponent);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "placeholder", _s_set_placeholder);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "verticalScrollbar", _s_set_verticalScrollbar);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "scrollSensitivity", _s_set_scrollSensitivity);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "caretColor", _s_set_caretColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "customCaretColor", _s_set_customCaretColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "selectionColor", _s_set_selectionColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onEndEdit", _s_set_onEndEdit);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onSubmit", _s_set_onSubmit);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onSelect", _s_set_onSelect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onDeselect", _s_set_onDeselect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onTextSelection", _s_set_onTextSelection);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onEndTextSelection", _s_set_onEndTextSelection);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onValueChanged", _s_set_onValueChanged);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onTouchScreenKeyboardStatusChanged", _s_set_onTouchScreenKeyboardStatusChanged);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onValidateInput", _s_set_onValidateInput);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "characterLimit", _s_set_characterLimit);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pointSize", _s_set_pointSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "fontAsset", _s_set_fontAsset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onFocusSelectAll", _s_set_onFocusSelectAll);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "resetOnDeActivation", _s_set_resetOnDeActivation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "restoreOriginalTextOnEscape", _s_set_restoreOriginalTextOnEscape);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "isRichTextEditingAllowed", _s_set_isRichTextEditingAllowed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "contentType", _s_set_contentType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "lineType", _s_set_lineType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "lineLimit", _s_set_lineLimit);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "inputType", _s_set_inputType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "keyboardType", _s_set_keyboardType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "characterValidation", _s_set_characterValidation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "inputValidator", _s_set_inputValidator);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "readOnly", _s_set_readOnly);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "richText", _s_set_richText);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "asteriskChar", _s_set_asteriskChar);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "caretPosition", _s_set_caretPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "selectionAnchorPosition", _s_set_selectionAnchorPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "selectionFocusPosition", _s_set_selectionFocusPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "stringPosition", _s_set_stringPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "selectionStringAnchorPosition", _s_set_selectionStringAnchorPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "selectionStringFocusPosition", _s_set_selectionStringFocusPosition);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "TMPro.TMP_InputField does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTextWithoutNotify(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _input = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.SetTextWithoutNotify( _input );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveTextEnd(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _shift = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.MoveTextEnd( _shift );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveTextStart(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _shift = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.MoveTextStart( _shift );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveToEndOfLine(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _shift = LuaAPI.lua_toboolean(L, 2);
                    bool _ctrl = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.MoveToEndOfLine( _shift, _ctrl );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveToStartOfLine(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _shift = LuaAPI.lua_toboolean(L, 2);
                    bool _ctrl = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.MoveToStartOfLine( _shift, _ctrl );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnBeginDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnBeginDrag( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnDrag( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEndDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnEndDrag( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnPointerDown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnPointerDown( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ProcessEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Event _e = (UnityEngine.Event)translator.GetObject(L, 2, typeof(UnityEngine.Event));
                    
                    gen_to_be_invoked.ProcessEvent( _e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnUpdateSelected(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.BaseEventData _eventData = (UnityEngine.EventSystems.BaseEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.BaseEventData));
                    
                    gen_to_be_invoked.OnUpdateSelected( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnScroll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnScroll( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ForceLabelUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ForceLabelUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Rebuild(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.UI.CanvasUpdate _update;translator.Get(L, 2, out _update);
                    
                    gen_to_be_invoked.Rebuild( _update );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LayoutComplete(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LayoutComplete(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GraphicUpdateComplete(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.GraphicUpdateComplete(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ActivateInputField(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ActivateInputField(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnSelect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.BaseEventData _eventData = (UnityEngine.EventSystems.BaseEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.BaseEventData));
                    
                    gen_to_be_invoked.OnSelect( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnPointerClick(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnPointerClick( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnControlClick(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnControlClick(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReleaseSelection(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ReleaseSelection(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeactivateInputField(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _clearSelection = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.DeactivateInputField( _clearSelection );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.DeactivateInputField(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TMPro.TMP_InputField.DeactivateInputField!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDeselect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.BaseEventData _eventData = (UnityEngine.EventSystems.BaseEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.BaseEventData));
                    
                    gen_to_be_invoked.OnDeselect( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnSubmit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.BaseEventData _eventData = (UnityEngine.EventSystems.BaseEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.BaseEventData));
                    
                    gen_to_be_invoked.OnSubmit( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalculateLayoutInputHorizontal(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CalculateLayoutInputHorizontal(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalculateLayoutInputVertical(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CalculateLayoutInputVertical(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetGlobalPointSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _pointSize = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.SetGlobalPointSize( _pointSize );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetGlobalFontAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    TMPro.TMP_FontAsset _fontAsset = (TMPro.TMP_FontAsset)translator.GetObject(L, 2, typeof(TMPro.TMP_FontAsset));
                    
                    gen_to_be_invoked.SetGlobalFontAsset( _fontAsset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_shouldHideMobileInput(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.shouldHideMobileInput);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_shouldHideSoftKeyboard(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.shouldHideSoftKeyboard);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_text(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.text);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isFocused(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isFocused);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_caretBlinkRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.caretBlinkRate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_caretWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.caretWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_textViewport(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.textViewport);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_textComponent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.textComponent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_placeholder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.placeholder);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_verticalScrollbar(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.verticalScrollbar);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scrollSensitivity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.scrollSensitivity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_caretColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.caretColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_customCaretColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.customCaretColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_selectionColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.selectionColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onEndEdit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onEndEdit);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onSubmit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onSubmit);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onSelect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onSelect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onDeselect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onDeselect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onTextSelection(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onTextSelection);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onEndTextSelection(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onEndTextSelection);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onValueChanged(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onValueChanged);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onTouchScreenKeyboardStatusChanged(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onTouchScreenKeyboardStatusChanged);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onValidateInput(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onValidateInput);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_characterLimit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.characterLimit);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pointSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.pointSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_fontAsset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.fontAsset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onFocusSelectAll(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.onFocusSelectAll);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_resetOnDeActivation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.resetOnDeActivation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_restoreOriginalTextOnEscape(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.restoreOriginalTextOnEscape);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isRichTextEditingAllowed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isRichTextEditingAllowed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_contentType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.contentType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_lineType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.lineType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_lineLimit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.lineLimit);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_inputType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.inputType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_keyboardType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.keyboardType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_characterValidation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.characterValidation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_inputValidator(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.inputValidator);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_readOnly(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.readOnly);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_richText(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.richText);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_multiLine(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.multiLine);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_asteriskChar(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.asteriskChar);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_wasCanceled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.wasCanceled);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_caretPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.caretPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_selectionAnchorPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.selectionAnchorPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_selectionFocusPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.selectionFocusPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stringPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.stringPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_selectionStringAnchorPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.selectionStringAnchorPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_selectionStringFocusPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.selectionStringFocusPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_minWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.minWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_preferredWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.preferredWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_flexibleWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.flexibleWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_minHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.minHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_preferredHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.preferredHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_flexibleHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.flexibleHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layoutPriority(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.layoutPriority);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_shouldHideMobileInput(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.shouldHideMobileInput = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_shouldHideSoftKeyboard(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.shouldHideSoftKeyboard = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_text(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.text = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_caretBlinkRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.caretBlinkRate = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_caretWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.caretWidth = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_textViewport(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.textViewport = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_textComponent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.textComponent = (TMPro.TMP_Text)translator.GetObject(L, 2, typeof(TMPro.TMP_Text));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_placeholder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.placeholder = (UnityEngine.UI.Graphic)translator.GetObject(L, 2, typeof(UnityEngine.UI.Graphic));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_verticalScrollbar(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.verticalScrollbar = (UnityEngine.UI.Scrollbar)translator.GetObject(L, 2, typeof(UnityEngine.UI.Scrollbar));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_scrollSensitivity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.scrollSensitivity = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_caretColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.caretColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_customCaretColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.customCaretColor = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_selectionColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.selectionColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onEndEdit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onEndEdit = (TMPro.TMP_InputField.SubmitEvent)translator.GetObject(L, 2, typeof(TMPro.TMP_InputField.SubmitEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onSubmit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onSubmit = (TMPro.TMP_InputField.SubmitEvent)translator.GetObject(L, 2, typeof(TMPro.TMP_InputField.SubmitEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onSelect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onSelect = (TMPro.TMP_InputField.SelectionEvent)translator.GetObject(L, 2, typeof(TMPro.TMP_InputField.SelectionEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onDeselect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onDeselect = (TMPro.TMP_InputField.SelectionEvent)translator.GetObject(L, 2, typeof(TMPro.TMP_InputField.SelectionEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onTextSelection(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onTextSelection = (TMPro.TMP_InputField.TextSelectionEvent)translator.GetObject(L, 2, typeof(TMPro.TMP_InputField.TextSelectionEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onEndTextSelection(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onEndTextSelection = (TMPro.TMP_InputField.TextSelectionEvent)translator.GetObject(L, 2, typeof(TMPro.TMP_InputField.TextSelectionEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onValueChanged(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onValueChanged = (TMPro.TMP_InputField.OnChangeEvent)translator.GetObject(L, 2, typeof(TMPro.TMP_InputField.OnChangeEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onTouchScreenKeyboardStatusChanged(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onTouchScreenKeyboardStatusChanged = (TMPro.TMP_InputField.TouchScreenKeyboardEvent)translator.GetObject(L, 2, typeof(TMPro.TMP_InputField.TouchScreenKeyboardEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onValidateInput(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onValidateInput = translator.GetDelegate<TMPro.TMP_InputField.OnValidateInput>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_characterLimit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.characterLimit = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pointSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.pointSize = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_fontAsset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.fontAsset = (TMPro.TMP_FontAsset)translator.GetObject(L, 2, typeof(TMPro.TMP_FontAsset));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onFocusSelectAll(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onFocusSelectAll = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_resetOnDeActivation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.resetOnDeActivation = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_restoreOriginalTextOnEscape(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.restoreOriginalTextOnEscape = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isRichTextEditingAllowed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.isRichTextEditingAllowed = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_contentType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                TMPro.TMP_InputField.ContentType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.contentType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_lineType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                TMPro.TMP_InputField.LineType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.lineType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_lineLimit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.lineLimit = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_inputType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                TMPro.TMP_InputField.InputType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.inputType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_keyboardType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                UnityEngine.TouchScreenKeyboardType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.keyboardType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_characterValidation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                TMPro.TMP_InputField.CharacterValidation gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.characterValidation = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_inputValidator(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.inputValidator = (TMPro.TMP_InputValidator)translator.GetObject(L, 2, typeof(TMPro.TMP_InputValidator));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_readOnly(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.readOnly = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_richText(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.richText = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_asteriskChar(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.asteriskChar = (char)LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_caretPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.caretPosition = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_selectionAnchorPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.selectionAnchorPosition = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_selectionFocusPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.selectionFocusPosition = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_stringPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.stringPosition = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_selectionStringAnchorPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.selectionStringAnchorPosition = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_selectionStringFocusPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPro.TMP_InputField gen_to_be_invoked = (TMPro.TMP_InputField)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.selectionStringFocusPosition = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
