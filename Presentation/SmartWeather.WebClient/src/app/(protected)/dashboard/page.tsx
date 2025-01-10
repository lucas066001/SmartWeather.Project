import MainLayout from "@/components/ui/mainLayout";
import SocketConnector from "@/components/ui/socket/socketConnector";

function DashboardPage() {
  return (
    <MainLayout title="Dashboard">
      <SocketConnector />
    </MainLayout>
  );
}

export default DashboardPage