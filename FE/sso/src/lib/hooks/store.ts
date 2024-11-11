import { useDispatch, useSelector, useStore } from "react-redux";
import { AppDispatch, AppStore, RootState } from "../store";

export const useAppDispatch = useDispatch<AppDispatch>;
export const useAppSelector = useSelector<RootState>;
export const useAppStore = useStore<AppStore>;
