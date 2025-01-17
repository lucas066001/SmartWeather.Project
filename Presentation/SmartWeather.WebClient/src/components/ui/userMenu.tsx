import { logout } from "@/actions/auth";
import { PropsWithChildren, useState } from "react";
import { twMerge } from "tailwind-merge";
import LineArrow from "../icons/lineArrow";
import LoadingCircle from "../icons/loadingCircle";
import Logout from "../icons/logout";
import Notifications from "../icons/notifications";
import UserIcon from "../icons/userIcon";
import CardLabel from "./cardLabel";

interface ICardUserMenuProps extends PropsWithChildren{
    className?: string
}

function UserMenu({className}: ICardUserMenuProps) {
    const [isOpened, setIsOpened] = useState<boolean>(false); // boolean used to check open state user menu
    const [isLoading, setIsLoading] = useState<boolean>(false); // boolean used to check open state user menu
    return (
        <div className={className}>
            <div className={twMerge("bg-primary opacity-100 rounded-[28px] p-2")}>
                <div className="flex gap-3 items-center">
                    <button className="bg-white rounded-[21px] p-1">
                        <div className="flex gap-2 items-center p-1">
                        <Notifications className="" />
                        </div>
                    </button>
                    <button className="bg-white rounded-[21px] p-1" onClick={() => setIsOpened(!isOpened)}>
                        <div className="flex gap-2 items-center p-1">
                            <UserIcon className="opacity-100"></UserIcon>
                            <LineArrow className="opacity-100" up={isOpened}></LineArrow>
                        </div>
                    </button>
                </div>
            </div>
                    {
                        isOpened &&
                        <CardLabel className={"absolute right-0 mt-2 mr-2 z-50"} label="User" contentClassName="ml-0">
                            <div className="flex flex-col items-center gap-4">
                                <button className="rounded-[6px] border-2 border-disabled p-1 rounded-lg" onClick={() => {
                                    setIsLoading(true);
                                    logout().then(() => setIsLoading(false));
                                    }}>
                                    <div className="flex gap-4 p-1">
                                        {!isLoading && <Logout className=""></Logout>}
                                        {isLoading && <LoadingCircle color="#B1B1B1"/>}
                                        <span className="text-disabled">Logout</span>
                                    </div>
                                </button>
                            </div>
                        </CardLabel>
                    }
        </div>
    )
}

export default UserMenu;